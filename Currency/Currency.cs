using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml;


namespace MyLib
{
    /// <summary>
    /// Представляет класс для получения бла-бла.
    /// </summary>
    public class Currency
    {



        /// <summary>
        /// Получает значение кода валюты.
        /// </summary>
        public ECurrencyType CurrencyType { get; }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="Currency"/>.
        /// </summary>
        /// <param name="currencyCode">Код валюты.</param>
        public Currency(ECurrencyType currencyCode) => CurrencyType = currencyCode;

        /// <summary>
        /// Курс по ЦБ Рф в диапозоне
        /// </summary>
        /// <param name="rangeTuple">Диапозон</param>
        /// <returns></returns>
        public IEnumerable<decimal> ValuesOfRange(DateTime FirsDateTime, DateTime LastDateTime)
        {
            var rootElement = GetXmlElement(MakeUri(FirsDateTime,LastDateTime));

            return rootElement.Cast<XmlNode>()
                .SelectMany(xNode => xNode.Cast<XmlNode>(), (xNode, child) => new {xNode, child})
                .Where(@t => @t.child.Name == "Value")
                .Select(@t => decimal.Parse(@t.child.InnerText));
        }

        private static T GetAttribute<T>(object value) where T :Attribute
        {
            var fieldInfo = value?.GetType().GetField(value.ToString());
            return fieldInfo?.GetCustomAttribute<T>();
        }


        /// <summary>
        /// Возвращает Uri бла-бла.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Uri бла-бла.</returns>
        private string MakeUri(DateTime startDate, DateTime endDate)
        {
            var currencyCode = GetAttribute<CurrencyCodeAttribute>(CurrencyType)?.Value ?? "";

            return "http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1=" +
                   startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "&date_req2=" +
                   endDate.AddDays(1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "&VAL_NM_RQ=" + currencyCode;
        }

        /// <summary>
        /// Делает xmlElement из ссылки на xml файл
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private static XmlElement GetXmlElement(string link)
        {
            var request = WebRequest.CreateHttp(link);
            var response = request.GetResponse();
            var xDoc = new XmlDocument();
            xDoc.Load(response.GetResponseStream());
            return xDoc.DocumentElement;
        }

    }
}
