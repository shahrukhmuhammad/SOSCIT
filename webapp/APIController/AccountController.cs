using BaseApp.Entity;
using BaseApp.System;
using CPC;
using CPC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Filters;

namespace WebApp.APIController
{
    [AllowCrossSiteJson]
    public class AccountController : ApiController
    {
        private VehicleEntity vehicleRepo;
        private OrderbookingEntity orderBookingRepo;
        public AccountController()
        {
            vehicleRepo = new VehicleEntity();
            orderBookingRepo = new OrderbookingEntity();
        }
        string AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1NjIiLCJqdGkiOiJkMjhhMTQ4ZC01MDJkLTQyYWUtYWUxMC03MWExM2Y4MmRmYTYiLCJ1c2VyLWlkIjoiNTYyIiwiZnVsbC1uYW1lIjoiQWRtaW5UZXN0IE1hemF5YVRlc3QiLCJlbWFpbC1hZGRyZXNzIjoiYWRtaW5AbWF6YXlhLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNTcxMzE5ODc0LCJpc3MiOiJodHRwOi8vbWF6YXlhLWFwaS1xYS5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6Imh0dHA6Ly9tYXpheWEtYXBpLXFhLmF6dXJld2Vic2l0ZXMubmV0In0.ROh_7the6UJj0V_Y5mU1eeGbqz1X2SO2ZYmclBvlbVM";

        [HttpPost]
        //[Route("api/Account/Login")]
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
                    if (res != null && res == "Login Successfully!")
                    {
                        return Json(new { error = "false", message = "Login Success!" });

                        //try
                        //{
                        //    //orderDetails = orderBookingRepo.GetOrderbyVehicleNumber(objUser.vehicleNumber);
                        //    return Json(new { error = "false", message = "Login Success!" });
                        //}
                        //catch (Exception ex)
                        //{
                        //    return Json(new { error = "true", message = ex.Message.ToString(), });
                        //}
                    }
                    else
                        return Json(new { error = "true", message = "Failed! User Does not exist" });
                }
                #endregion
            }
            return Json(new { error = "true", message = "Error! Provided AccessToken is not correct" });
        }

        [HttpPost]
        //[Route("api/Account/GetOrderDetails")]
        public IHttpActionResult GetOrderDetails([FromBody]VehicleLoginModel objUser)
        {
            var accesstoken = Request.Headers.GetValues("Authorization").FirstOrDefault();
            if (AccessToken == accesstoken)
            {
                #region Login
                if (!string.IsNullOrEmpty(objUser.vehicleNumber))
                {
                    var orderDetails = new Vew_Orderbookings();
                    try
                    {
                        orderDetails = orderBookingRepo.GetOrderbyVehicleNumber(objUser.vehicleNumber);

                        if (orderDetails != null)
                        {
                            var crewList = vehicleRepo.GetCrewListByVehicleId(orderDetails.VehicleId.Value);

                            return Json(new
                            {
                                error = "false",
                                message = "Order Exist",
                                details = new
                                {
                                    OrderId = orderDetails.Id,
                                    orderDetails.OrderNo,
                                    orderDetails.VehicleNumber,
                                    orderDetails.Date,
                                    orderDetails.ApprovedByName,
                                    orderDetails.ApprovedOn,
                                    orderDetails.CollectionBranchName,
                                    orderDetails.DeliveryBranchName,
                                    orderDetails.BilledBranchName,
                                    orderDetails.Status,
                                    StatusTitle = Convert.ToString((AnnexureStatus)orderDetails.Status),
                                    OrderType = Convert.ToString((OrderType)orderDetails.OrderType),
                                    TransportationMood = Convert.ToString((TransportationMood)orderDetails.TransportationMood).ToSpacedTitleCase(),
                                },
                                crewList = crewList.CITVehicleCrews.Select(x => new {
                                    x.EmployeeId,
                                    EmployeeName = x.CPCEmployee.Code + " " + x.CPCEmployee.Name,
                                    x.CPCEmployee.CNIC
                                }).ToList(),
                            });
                        }
                        else
                            return Json(new { error = "true", message = "No Order Found" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { error = "true", message = ex.Message.ToString() });
                    }
                }
                #endregion
            }
            return Json(new { error = "true", message = "Error! Provided AccessToken is not correct" });
        }

    }


}
