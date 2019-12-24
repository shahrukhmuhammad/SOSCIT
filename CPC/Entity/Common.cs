using CPC.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC
{
    public class Common
    {
        public string GetNextCode(string tableName)
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    return context.Database.SqlQuery<string>("exec SP_GetMaxCodeByTableName @TableName", new SqlParameter("TableName", tableName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //new Logger().LogError(ex);
                return null;
            }
        }

        //public List<CPCDenomination> GetAllDenominationDropdown()
        //{
        //    try
        //    {
        //        using (var context = new SOSTechCPCEntities())
        //        {
        //            return context.CPCDenominations.OrderBy(x => x.Id).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<CustomSelectList> GetAllDenominationDropdown()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    var ls = context.CPCDenominations.OrderBy(x=> x.DenominationTitle).ToList();
                    return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.DenominationTitle.ToString() }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Guid? GetDinomIdByName(int denomName)
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    var obj = context.CPCDenominations.Where(x => x.DenominationTitle == denomName).FirstOrDefault();
                    if (obj != null)
                    {
                        return obj.Id;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }

        public List<CPCDenomination> GetAllDenomination()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    return context.CPCDenominations.OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomSelectList> GetAllProjectsDropdown()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    var ls = context.CPCProjects.OrderBy(x => x.Name).ToList();
                    return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.Name.ToString() }).ToList();
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }

        public List<CustomSelectList> GetAllRegionsDropdown()
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    var ls = new List<CPCCity>();
                    ls = context.CPCCities.OrderBy(x => x.CityName).ToList();

                    return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.CityName.ToString() }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CustomSelectList> GetAllCitiesDropdown(Guid? RegionId = null)
        {
            try
            {
                using (var context = new SOSTechCPCEntities())
                {
                    var ls = new List<CPCCity>();
                    if (RegionId.HasValue)
                        ls = context.CPCCities.Where(x=> x.RegionId == RegionId).OrderBy(x => x.CityName).ToList();
                    else
                        ls = context.CPCCities.OrderBy(x => x.CityName).ToList();

                    return ls.Select(x => new CustomSelectList { Value = x.Id.ToString(), Text = x.CityName.ToString() }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }



    public class CustomSelectList
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Details { get; set; }
    }

    public enum AnnexureStatus
    {
        Inprocess = 1,
        Completed = 2,
        Approved = 3,
        Proceeded = 4,
        Rejected = 5,
        Sorted = 6,
        PendingDelivery = 7,
        Delivered = 8
    }

    public enum CashPoint
    {
        CPH = 1, //Cash Processing House
        Branch = 2,
    }

    //public enum BankStatus
    //{
    //    Inprocess = 1,
    //    Approved = 2,
    //    Completed = 3
    //}
}
