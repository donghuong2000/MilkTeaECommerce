
using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Utility;
using PayPal.Core;

using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkTeaECommerce.Controllers
{
    public class PaypalController : Controller
    {
        private readonly string _clientId;
        private readonly string _secretKey;
        private readonly ApplicationDbContext _db;
        public double TyGiaUSD = 23300;//store in Database
        public PaypalController(IConfiguration config, ApplicationDbContext db)
        {
            _db = db;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Previews()
        {
            return View();
        }

        //[Authorize]
        public async System.Threading.Tasks.Task<IActionResult> PaypalCheckout()
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            var cart = HttpContext.Session.Get<OrderHeader>("cart");
            #region Create Paypal Order
            
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };

            foreach (var item in cart.OrderDetails)
            {
                itemList.Items.Add(new Item()
                {
                    Name = item.Product.Name,//"Trà sữa",
                    Currency = "USD",
                    Price = Math.Round(item.Product.Price.GetValueOrDefault() / TyGiaUSD, 2).ToString(),
                    Quantity = item.Count.ToString(),
                    Description = "Số lượng: "+ item.Count.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
            }
            var total =itemList.Items.Sum(x=> double.Parse( x.Price)*int.Parse(x.Quantity));

            #endregion

            //var total = Math.Round(double.Parse(checkout[0].Total.ToString()) / 23000, 2) *itemList.Items.Count;

            var paypalOrderId = cart.Id;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        
                        Amount = new Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString()
                            }

                        },
                        ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/Paypal/CheckoutFail",
                    ReturnUrl = $"{hostname}/Paypal/CheckoutSuccess"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                #region Add cart
                cart.ApplicationUser = null;

                var newlistOrderDetail = cart.OrderDetails.Select(x => new OrderDetail
                {
                    Count = x.Count,
                    OrderHeaderId = x.OrderHeaderId,
                    ProductId = x.ProductId,
                    Price = x.Price,
                    Id = x.Id,
                    Status = x.Status,
                    DeliveryDetails = new DeliveryDetail() { Address = x.DeliveryDetails.Address, DateEnd = x.DeliveryDetails.DateEnd, Price = x.DeliveryDetails.Price, DateStart = x.DeliveryDetails.DateStart, DeliveryId = x.DeliveryDetails.DeliveryId, Note = x.DeliveryDetails.Note, OrderDetailId = x.DeliveryDetails.OrderDetailId }
                });
                cart.OrderDetails = newlistOrderDetail.ToList();


                foreach (var item in cart.OrderDetails)
                {
                    var objfromDb = _db.Products.Find(item.ProductId);
                    objfromDb.Quantity = objfromDb.Quantity - item.Count;
                    if (objfromDb.Quantity < 0)
                        throw new Exception("Lỗi đơn hàng " + objfromDb.Name + " chỉ còn " + (objfromDb.Quantity + item.Count));
                    else
                    {
                        _db.Update(objfromDb);
                        _db.SaveChanges();
                    }

                }
                _db.OrderHeaders.Add(cart);
                _db.SaveChanges();
                #endregion
                var response = await client.Execute(request);
                
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }
                await transaction.CommitAsync();
                transaction.Dispose();
                HttpContext.Session.Remove("cart");
                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                transaction.Rollback();
                transaction.Dispose();
                //Process when Checkout with Paypal fails
                return Redirect("/Paypal/CheckoutFail");
            }
            catch(Exception)
            {
                transaction.Rollback();
                transaction.Dispose();
                return Redirect("/Paypal/CheckoutFail");
            }
        }
        public IActionResult CheckoutFail()
        {
            //Tạo đơn hàng trong database với trạng thái thanh toán là "Chưa thanh toán"
            //Xóa session
            return Content("Thanh toán không thành công");
        }

        public IActionResult CheckoutSuccess()
        {
            //Tạo đơn hàng trong database với trạng thái thanh toán là "Paypal" và thành công
            //Xóa session
            return View();
           
        }
       
    }
}
