using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ServerConnection
    {
        private SAPbobsCOM.Company company = new SAPbobsCOM.Company();
        private int connectionResult;
        private int errorCode = 0;
        private string errorMessage = "";

        public int Connect()
        {
            company.Server = ConfigurationManager.AppSettings["server"].ToString();
            company.CompanyDB = ConfigurationManager.AppSettings["companydb"].ToString();
            company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
            company.DbUserName = ConfigurationManager.AppSettings["dbuser"].ToString();
            company.DbPassword = ConfigurationManager.AppSettings["dbpassword"].ToString();
            company.UserName = ConfigurationManager.AppSettings["user"].ToString();
            company.Password = ConfigurationManager.AppSettings["password"].ToString();
            company.language = SAPbobsCOM.BoSuppLangs.ln_English_Gb;
            company.UseTrusted = false;
            company.LicenseServer = ConfigurationManager.AppSettings["licenseServer"].ToString();

            connectionResult = company.Connect();

            if (connectionResult != 0)
            {
                company.GetLastError(out errorCode, out errorMessage);
            }

            return connectionResult;
        }
        public SAPbobsCOM.Company GetCompany()
        {
            return this.company;
        }
        public int GetErrorCode()
        {
            return this.errorCode;
        }
        public String GetErrorMessage()
        {
            return this.errorMessage;
        }
    }
}