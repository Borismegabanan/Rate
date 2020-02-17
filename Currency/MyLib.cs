using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Xml;


namespace MyLib
{
    public class Currency
    {

        //readonly
        //dollar code R01235
        public string CurrencyCode { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="currencyCode"></param>
        public Currency(string currencyCode) => CurrencyCode = currencyCode;

        /// <summary>
        /// Курс по ЦБ Рф в диапозоне
        /// </summary>
        /// <param name="rangeTuple">Диапозон</param>
        /// <returns></returns>
        public IEnumerable<decimal> ValuesOfRange(DateTime FirsDateTime, DateTime LastDateTime)
        {
            List<decimal> values = new List<decimal>();
            var xRoot = GetXmlElement(MakeUri(FirsDateTime,LastDateTime));
            foreach (XmlNode xNode in xRoot)
            {
                foreach (XmlNode child in xNode)
                {
                    if (child.Name == "Value")
                        values.Add(decimal.Parse(child.InnerText));
                }
            }
            return values;
        }


        /// <summary>
        /// Создаёт ссылку на ЦБ РФ
        /// </summary>
        /// <param name="FirstDate"></param>
        /// <param name="SecondDate"></param>
        /// <returns></returns>
        private string MakeUri(DateTime FirstDate,DateTime SecondDate)
        {
            return "http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1=" +
                   FirstDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "&date_req2=" +
                   SecondDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "&VAL_NM_RQ="+CurrencyCode;
        }

        /// <summary>
        /// Делает xmlElement из ссылки на xml файл
        /// </summary>
        /// <param name="Link"></param>
        /// <returns></returns>
        private XmlElement GetXmlElement(string Link)
        {
            var request = WebRequest.CreateHttp(Link);
            var response = request.GetResponse();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(response.GetResponseStream());
            return xDoc.DocumentElement;
        }

    }
}
