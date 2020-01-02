using BaseApp.Entity;
using CPC;
using CPC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.APIController
{

    public class LoginController : ApiController
    {
        private VehicleEntity vehicleRepo;
        private OrderbookingEntity orderBookingRepo;
        public LoginController()
        {
            vehicleRepo = new VehicleEntity();
            orderBookingRepo = new OrderbookingEntity();
        }
        string AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NjIiLCJqdGkiOiJkMjhhMTQ4ZC01MDJkLTQyYWUtYWUxMC03MWExM2Y4MmRmYTYiLCJ1c2VyLWlkIjoiNTYyIiwiZnVsbC1uYW1lIjoiQWRtaW5UZXN0IE1hemF5YVRlc3QiLCJlbWFpbC1hZGRyZXNzIjoiYWRtaW5AbWF6YXlhLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNTcxMzE5ODc0LCJpc3MiOiJodHRwOi8vbWF6YXlhLWFwaS1xYS5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6Imh0dHA6Ly9tYXpheWEtYXBpLXFhLmF6dXJld2Vic2l0ZXMubmV0In0.ROh_7the6UJj0V_Y5mU1eeGbqz1X2SO2ZYmclBvlbVM";
        [HttpPost]
        public IHttpActionResult Login([FromBody]VehicleLoginModel objUser)
        {
            var accesstoken = Request.Headers.GetValues("Authorization").FirstOrDefault();

            if (AccessToken == accesstoken)
            {
                #region Login
                if (!string.IsNullOrEmpty(objUser.vehicleNumber) && !string.IsNullOrEmpty(objUser.password))
                {
                    var res = vehicleRepo.AuthnticateVehicle(objUser);
                    var orderDetails = new Vew_Orderbookings();
                    if (res != null)
                    {
                        try
                        {
                            orderDetails = orderBookingRepo.GetOrderbyVehicleNumber(Guid.Parse(objUser.vehicleNumber));
                        }
                        catch (Exception ex)
                        {
                            return Json(new { error = "true", message = ex.Message.ToString(), });
                        }
                        return Json(new
                        {
                            error = "false",
                            message = res,
                            details = new
                            {
                                orderDetails.OrderNo,
                                orderDetails.Date,
                                orderDetails.ApprovedByName,
                                orderDetails.Date,
                                orderDetails.Date,
                            }
                        });
                    }
                    else
                        return Json(new { error = "true", message = "Failed! User Does not exist" });
                }
                #endregion
            }
            return Json(new { error = "true", message = "Error! Provided AccessToken is not correct" });
        }

        [HttpGet]
        public IHttpActionResult LoginView()
        {
            var accesstoken = Request.Headers.GetValues("Authorization").FirstOrDefault();

            //if (AccessToken == accesstoken)
            //{
            //    #region Login
            //    if (!string.IsNullOrEmpty(objUser.vehicleNumber) && !string.IsNullOrEmpty(objUser.password))
            //    {
            //        var backVehicle = vehicleRepo.AuthnticateVehicle(objUser);

            //        if (backVehicle != null)
            //        {
            //            return Json(new { error = "false", message = "Success!", backVehicle });
            //        }
            //        else
            //            return Json(new { error = "true", message = "Failed! User Does not exist" });
            //    }
            //    #endregion
            //}
            return Json(new { error = "true", message = "Error! Provided AccessToken is not correct" });
        }
    }


}
