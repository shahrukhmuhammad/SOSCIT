using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPC
{
    public static class Constants
    {
        public const string ACCESS_TOKEN = "60B1F227-0DC8-4D76-9A67-8C1061A795F0";

        //public const string UPLOADED_FILES = "UploadedFiles";
        //public const string UPLOADED_SalesOperationsDOCUMENTS_PATH = "UploadedFiles/SalesOperationsDocuments/";
        //public const string OPERATIONALMESSAGE = "OperationalMessage";
        //public const string PAGE_TITLE = "pageTitle";


        //public const string SMTP_SERVER_KEY = "";
        //public const string SMTP_PORT_KEY = "25";
        //public const string SMTP_SSL_KEY = "false";
        //public const string SMTP_AUTH_KEY = "true";
        //public const string SMTP_USER_NAME_KEY = "";
        //public const string SMTP_USER_EMAIL_KEY = "";
        //public const string SMTP_USER_PASS_KEY = "";

        //public const string DATE_FORMAT_SERVER_SIDE = "dd-MMM-yyyy";
        //public const string DATE_FORMAT_CLIENT_SIDE = "dd-MM-yyyy";
        //public const string DATE_FORMAT_MASK_CLIENT_SIDE = "99-aaa-9999";

        public const string DATE_FORMAT_CLIENT_SIDE = "dd/MM/yyyy";
        public const string DATE_FORMAT_MASK_CLIENT_SIDE = "99/99/9999";
        public const string PHONE_FORMAT_MASK = "0399-9999999";
        public const string CNIC_FORMAT_MASK = "99999-9999999-9";

        public const int YEAR_RANGE_START = 1980;
        public const int YEAR_RANGE_END = 2050;

        public const string REPORT_PATH = "../../SOReports/";

        public const string LOADING_GIF =
            @"<div style='text-align:center'><h2><i class='fa fa-spinner fa-spin fa-2x'></i>&nbsp;&nbsp;Loading... Please wait...!</h2></div>";

    }
}
