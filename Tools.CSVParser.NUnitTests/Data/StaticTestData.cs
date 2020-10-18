using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.CSVParser.NUnitTests.Data
{
    public static class StaticTestData
    {

        public static readonly string StaticTypesData = @"ID;NAME;ENABLED;BIRTHDATE;BALANCE;HEIGHT;AGE;SECONDS
0;Kamil Karpinski;true;1994.12.05;199.99;1.83;26;33321600
1;Jan Kowalski;false;1988.01.01;501.64;1.70;32;41011200
2;Aleksander Nowak;1;1992.04.10;306.59;1.80;28;36792000
3;Michał Wójcik;0;1977.10.12;720.44;1.89;43;56502000";

        public static readonly string NullableTypesData = @"ID;NAME;ENABLED;BIRTHDATE;BALANCE;HEIGHT;AGE;SECONDS
0;;true;1994.12.05;199.99;1.83;26;33321600
1;Jan Kowalski;;1988.01.01;501.64;1.70;32;41011200
2;Aleksander Nowak;1;;306.59;1.80;28;36792000
3;Michał Wójcik;0;1977.10.12;;1.89;43;56502000
4;Filip Wojciechowski;0;1989.07.01;450;;31;40734000
5;Kazimierz Góralczyk;0;1960.05.16;105.16;1.82;;78840000
6;Zbigniew Kaczorek;0;1993.11.04;354.87;1.77;27;";

        public static readonly string ListTypesData = @"ID;NAME;ENABLED;BIRTHDATE;BALANCE;HEIGHT;AGE;SECONDS
0;Kamil,Weronika,Agata,Katarzyna;true,false,1,0;1994.12.05,1997.01.01,1995.02.10,1996.12.09;300.99,132.54,215.30,154.12;1.83,1.65,1.79,1.68;26,23,25,21;33321600,30222000,23850000,27594000,
1;Natalia;false;1992.01.01;150.66;1.56;28;36792000
2;;;;;;;";

        public static readonly string StaticTypesDataNoHeaders = @"0;Kamil Karpinski;true;1994.12.05;199.99;1.83;26;33321600
1;Jan Kowalski;false;1988.01.01;501.64;1.70;32;41011200
2;Aleksander Nowak;1;1992.04.10;306.59;1.80;28;36792000
3;Michał Wójcik;0;1977.10.12;720.44;1.89;43;56502000";

        public static readonly string StaticUndersizedData = @"ID;NAME
0;Kamil
1;Jan;Kowalski;32
2;Aleksander;Nowak";

        public static readonly string StaticUndersizedDataNoHeaders = @"0;Kamil
1;Jan;Kowalski;32
2;Aleksander;Nowak";

        public static readonly string StaticLeakyData = @"ID;NAME;SURNAME;AGE
;Kamil;Karpiński;26
1;;Kowalski;32
2;Aleksander;;28
3;Michał;Wójcik";

        public static readonly string StaticLeakyDataNoHeaders = @";Kamil;Karpiński;26
1;;Kowalski;32
2;Aleksander;;28
3;Michał;Wójcik";

        public static readonly string StaticAutoConvertDateTimeData = @"ID;NAME;DATETIME
0;Date;05.12.1994
1;DateTime;01.01.1997 00:05:00
2;Time;09:41
3;TimeSeconds;06:30:15";

        public static readonly string StaticManualConvertDateTimeData = @"ID;NAME;DATETIME
0;K Test;1994.12.05
1;J Test;1997.01.01
2;C Test;1999.08.27
3;D Test;1995.02.10";

        public static readonly string StaticTimeSpanConvertDateTimeData = @"ID;NAME;DATETIME
0;K Test;786582000
1;J Test;852073200
2;C Test;935704800
3;D Test;792370800";

    }
}
