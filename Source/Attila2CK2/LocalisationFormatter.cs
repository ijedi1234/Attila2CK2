using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class LocalisationFormatter {

        public static string header() {
            return "#CODE;ENGLISH;FRENCH;GERMAN;;SPANISH;;;;;;;;;x";
        }

        public static string format(string id, string english) {
            return format(id, english, english, english, english);
        }

        public static string format(string id, string english, string french, string german, string spanish) {
            string formattedStr = id;
            formattedStr += ";" + english;
            formattedStr += ";" + french;
            formattedStr += ";" + german;
            formattedStr += ";;" + spanish;
            formattedStr += ";;;;;;;;;x";
            return formattedStr;
        }

    }
}
