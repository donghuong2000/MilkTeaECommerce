using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public double TyGiaUSD = 23300;//store in Database
        public PaypalController(IConfiguration config)
        {
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


            #region Create Paypal Order
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            var total = Math.Round(30000.0 / TyGiaUSD, 2);
            itemList.Items.Add(new Item()
            {
                Name = "Trà sữa",
                Currency = "USD",
                Price = Math.Round(30000.0 / TyGiaUSD, 2).ToString(),
                Quantity = "1",
                Description = "Số lượng: 1",
                Sku = "sku",
                Tax = "0"
            });
            #endregion

            //var total = Math.Round(double.Parse(checkout[0].Total.ToString()) / 23000, 2) *itemList.Items.Count;

            var paypalOrderId = DateTime.Now.Ticks;
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

            try
            {
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

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
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
            return Content("Thanh toán thành công");
        }
    }
}
