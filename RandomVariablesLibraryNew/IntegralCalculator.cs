﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew
{
    public static class IntegralCalculator
    {
        /// <summary>
        /// Вычисляет интеграл функции на заданном интервале.
        /// </summary>
        /// <param name="a">Начало интервала интегрирования</param>
        /// <param name="b">Конец интервала интегрирования</param>
        /// <param name="integrand">Подынтегральная функция</param>
        /// <returns></returns>
        public static double Integrate(double a, double b, Func<double, double> integrand)
        {
            var integralValue = default(double);

            // конечный интервал [a,b]
            var isDefiniteIntegral = !double.IsInfinity(a) && !double.IsInfinity(b);
            if (isDefiniteIntegral)
            {
                if (a != 0 && b != 0)
                {
                    if (b/a > 10 && a > 0 && b > a)
                    {
                        var expWide = b / a;
                        var numberOfIntervals = Math.Ceiling(Math.Log10(expWide));
                        var nodes = LogSpace(Math.Log10(a), Math.Log10(b), (int)numberOfIntervals + 1).ToList();
                        nodes[0] = a;
                        nodes[nodes.Count - 1] = b; 
                        for (var i = 0; i < numberOfIntervals; i++)
                        {
                            integralValue += IntegrateByGaussLegendreQuadrature(nodes[i], nodes[i + 1], integrand);
                        }
                    }
                    else if (b / a < 0.1 && a < b && b < 0)
                    {
                        var expWide = a / b;
                        var numberOfIntervals = Math.Ceiling(Math.Log10(expWide));
                        var nodes = LogSpace(Math.Log10(Math.Abs(a)), Math.Log10(Math.Abs(b)), (int)numberOfIntervals + 1).ToList();
                        nodes = nodes.Select(n => (-1) * n).ToList();
                        nodes[0] = a;
                        nodes[nodes.Count - 1] = b;
                        for (var i = 0; i < numberOfIntervals; i++)
                        {
                            integralValue += IntegrateByGaussLegendreQuadrature(nodes[i], nodes[i + 1], integrand);
                        }
                    }
                    else
                    {
                        integralValue = IntegrateByGaussLegendreQuadrature(a, b, integrand);
                    }
                }
                else
                {
                    integralValue = IntegrateByGaussLegendreQuadrature(a, b, integrand);
                }
                //integralValue = IntegrateByGaussLegendreQuadrature(from, to, integrand);
            }

            // интервал (-inf; b], [a; +inf) или (-inf; +inf).
            if (double.IsInfinity(a) || double.IsInfinity(b))
            {
                integralValue = IntegrateWithVariableChange(a, b, integrand);
            }

            return integralValue;
        }

        private static IEnumerable<double> LogSpace(double start, double stop, int num, bool endpoint = true, double numericBase = 10.0d)
        {
            var y = LinSpace(start, stop, num: num, endpoint: endpoint);
            return Power(y, numericBase);
        }

        private static IEnumerable<double> Arange(double start, int count)
        {
            return Enumerable.Range((int)start, count).Select(v => (double)v);
        }

        private static IEnumerable<double> Power(IEnumerable<double> exponents, double baseValue = 10.0d)
        {
            return exponents.Select(v => Math.Pow(baseValue, v));
        }

        private static IEnumerable<double> LinSpace(double start, double stop, int num, bool endpoint = true)
        {
            var result = new List<double>();
            if (num <= 0)
            {
                return result;
            }

            if (endpoint)
            {
                if (num == 1)
                {
                    return new List<double>() { start };
                }

                var step = (stop - start) / ((double)num - 1.0d);
                result = Arange(0, num).Select(v => (v * step) + start).ToList();
            }
            else
            {
                var step = (stop - start) / (double)num;
                result = Arange(0, num).Select(v => (v * step) + start).ToList();
            }

            return result;
        }

        /// <summary>
        /// Вычисляет интеграл функции на заданном интервале с помощью замены переменной.
        /// </summary>
        /// <param name="from">Начало интервала интегрирования</param>
        /// <param name="to">Конец интервала интегрирования</param>
        /// <param name="integrand">Подынтегральная функция</param>
        /// <returns></returns>
        public static double IntegrateWithVariableChange(double from, double to, Func<double, double> integrand)
        {
            // Случай 1. Интеграл с бесконечным верхним пределом [a; +inf).
            var isInfiniteUpperLimit = !double.IsInfinity(from) && double.IsPositiveInfinity(to);
            if (isInfiniteUpperLimit)
            {
                return CalculateToPositiveInfinityIntegral(from, integrand);
            }

            // Случай 2. Интеграл с бесконечным нижним пределом (-inf; b].
            // Меняем пределы интегрирования местами: (-inf; b] -> [-b, +inf).
            // Подынтегральная функция f(x) заменяется на g(s), так что x=-s и g(s)=f(s).
            var isInfiniteLowerLimit = double.IsNegativeInfinity(from) && !double.IsInfinity(to);
            if (isInfiniteLowerLimit)
            {
                //Func<double, double> newFunc = (x) => integrand(-x);

                //var toPosInfIntegral = CalculateToPositiveInfinityIntegral((-1) * to, newFunc);
                //return toPosInfIntegral;

                var fromMinusInfIntegral = CalculateFromMinusInfinityIntegral(to, integrand);
                return fromMinusInfIntegral;
            }

            // Случай 3. (-inf; +inf)
            var isInfiniteLimits = double.IsNegativeInfinity(from) && double.IsPositiveInfinity(to);
            if (isInfiniteLimits)
            {
                var result = CalculateFromMinusInfinityIntegral(0, integrand) + CalculateToPositiveInfinityIntegral(0, integrand);
                return result;
            }

            return default;
        }

        /// <summary>
        /// Вычисляет интеграл с бесконечным нижним пределом (-inf; b].
        /// </summary>
        /// <param name="to">Конец отрезка интегрирования</param>
        /// <param name="integrand">Подынтегральная функция</param>
        /// <returns></returns>
        public static double CalculateFromMinusInfinityIntegral(double to, Func<double, double> integrand)
        {
            // Интеграл с бесконечным нижним пределом (-inf; b].
            // Меняем пределы интегрирования местами: (-inf; b] -> [-b, +inf).
            // Подынтегральная функция f(x) заменяется на g(s), так что x=-s и g(s)=f(s).

            Func<double, double> newFunc = (x) => integrand(-x);
            var toPosInfIntegral = CalculateToPositiveInfinityIntegral((-1) * to, newFunc);

            return toPosInfIntegral;
        }

        /// <summary>
        /// Вычисляет интеграл с бесконечным верхним пределом [a; +inf).
        /// </summary>
        /// <param name="to">Начало отрезка интегрирования</param>
        /// <param name="integrand">Подынтегральная функция</param>
        /// <returns></returns>
        public static double CalculateToPositiveInfinityIntegral(double from, Func<double, double> integrand)
        {
            // Если a < 0, то разбиваем интеграл на два по интервалам [a; 0] и [0; +inf).
            // Интеграл с пределами [0; +inf) вычисляется заменой переменной x = z / (1 - z) + 0,
            // причем после замены интеграл имеет пределы [0; 1].
            if (from < 0)
            {
                var funcAfterVariableChange = GetIntegrandAfterVariableChange(0, integrand);
                return IntegrateByGaussLegendreQuadrature(from, 0, integrand) + IntegrateByGaussLegendreQuadrature(0, 1, funcAfterVariableChange);
            }
            else // from >= 0
            {
                // Если a >= 0, то делаем замену переменной x = z / (1 - z) + a;
                var funcAfterVariableChange = GetIntegrandAfterVariableChange(from, integrand);
                return IntegrateByGaussLegendreQuadrature(0, 1, funcAfterVariableChange);
            }
        }

        private static Func<double, double> GetIntegrandAfterVariableChange(double from, Func<double, double> integrand)
        {
            return (z) => integrand((z / (1 - z)) + from) / Math.Pow(1 - z, 2);
        }

        /// <summary>
        /// Вычисляет интеграл с помощью квадратуры Гаусса-Лежандра
        /// </summary>
        /// <param name="a">Начало отрезка интегрирования</param>
        /// <param name="b">Конец отрезка интегрирования</param>
        /// <param name="function">Подынтегральная функция</param>
        /// <returns></returns>
        private static double IntegrateByGaussLegendreQuadrature(double a, double b, Func<double, double> function)
        {
            var n = 32; // количество точек
            var weights = new double[]
            {
                0.0965400885147278005667648300635757947368606312355700687323182099577497758679466512968173871061464644599963197828969869820251559172455698832434930732077927850876632725829187045819145660710266452161095406358159608874152584850413283587913891015545638518881205600825069096855488296437485836866,
    0.0965400885147278005667648300635757947368606312355700687323182099577497758679466512968173871061464644599963197828969869820251559172455698832434930732077927850876632725829187045819145660710266452161095406358159608874152584850413283587913891015545638518881205600825069096855488296437485836866,
    0.0956387200792748594190820022041311005948905081620055509529898509437067444366006256133614167190847508238474888230077112990752876436158047205555474265705582078453283640212465537132165041268773645168746774530146140911679782502276289938840330631903789120176765314495900053061764438990021439069,
    0.0956387200792748594190820022041311005948905081620055509529898509437067444366006256133614167190847508238474888230077112990752876436158047205555474265705582078453283640212465537132165041268773645168746774530146140911679782502276289938840330631903789120176765314495900053061764438990021439069,
    0.0938443990808045656391802376681172600361000757462364500506275696355695118623098075097804207682530277555307864917078828352419853248607668520631751470962234105835015158485760721979732297206950719908744248285672032436598213262204039212897239890934116841559005147755270269705682414708355646603,
    0.0938443990808045656391802376681172600361000757462364500506275696355695118623098075097804207682530277555307864917078828352419853248607668520631751470962234105835015158485760721979732297206950719908744248285672032436598213262204039212897239890934116841559005147755270269705682414708355646603,
    0.0911738786957638847128685771116370625448614132753900053231278739777031520613017513597426417145878622654027367650308019870251963114683369110451524174258161390823876554910693202594383388549640738095422966058367070348943662290656339592299608558384147559830707904449930677260444604329157917977,
    0.0911738786957638847128685771116370625448614132753900053231278739777031520613017513597426417145878622654027367650308019870251963114683369110451524174258161390823876554910693202594383388549640738095422966058367070348943662290656339592299608558384147559830707904449930677260444604329157917977,
    0.0876520930044038111427714627518022875484497217017572223192228034747061150211380239263021665771581379364685191248848158059408000065275041643745927401342920150588893827207354226012701872322225514682178439577327346929209121046816487338309068375228210705166692551938339727096609740531893725675,
    0.0876520930044038111427714627518022875484497217017572223192228034747061150211380239263021665771581379364685191248848158059408000065275041643745927401342920150588893827207354226012701872322225514682178439577327346929209121046816487338309068375228210705166692551938339727096609740531893725675,
    0.0833119242269467552221990746043486115387468839428344598401864047287594069244380966536255650452315042012372905572506028852130723585016898197140339352228963465326746426938359210160503509807644396182380868089959855742801355208471205261406307895519604387550841954817025499019984032594036141439,
    0.0833119242269467552221990746043486115387468839428344598401864047287594069244380966536255650452315042012372905572506028852130723585016898197140339352228963465326746426938359210160503509807644396182380868089959855742801355208471205261406307895519604387550841954817025499019984032594036141439,
    0.078193895787070306471740918828306671039786798482159190307481553869493700115196435401943819761440851294456424770323467367505109006517482028994114252939401250416132320553639542341400437522236191275346323130525969269563653003188829786549728825182082678498917784036375053244425839341945385297,
    0.078193895787070306471740918828306671039786798482159190307481553869493700115196435401943819761440851294456424770323467367505109006517482028994114252939401250416132320553639542341400437522236191275346323130525969269563653003188829786549728825182082678498917784036375053244425839341945385297,
    0.0723457941088485062253993564784877916043369833018248707397632823511765345816800402874475958591657429073027694582930574378890633404841054620298756279975430795706338162404545590689277985270140590721779502609564199074051863640176937117952488466002340085264819537808079947788437998042296495822,
    0.0723457941088485062253993564784877916043369833018248707397632823511765345816800402874475958591657429073027694582930574378890633404841054620298756279975430795706338162404545590689277985270140590721779502609564199074051863640176937117952488466002340085264819537808079947788437998042296495822,
    0.0658222227763618468376500637069387728775364473732465153710916696852412442018627316280044447764609054151761388378861151807154113495715653711918644796313239555117970398473141615070299152284100887258072240524028885129828725430021172354299810423059697133688823072212214503334259555369485963074,
    0.0658222227763618468376500637069387728775364473732465153710916696852412442018627316280044447764609054151761388378861151807154113495715653711918644796313239555117970398473141615070299152284100887258072240524028885129828725430021172354299810423059697133688823072212214503334259555369485963074,
    0.0586840934785355471452836373001708867501204674575467587150032786132877518019090643743123653437052116901895704813134467814193905269714480573030647540887991405215103758723074481312705449946311993670933802369300463315125015975216910705047901943865293781921122370996257470349807212516159332678,
    0.0586840934785355471452836373001708867501204674575467587150032786132877518019090643743123653437052116901895704813134467814193905269714480573030647540887991405215103758723074481312705449946311993670933802369300463315125015975216910705047901943865293781921122370996257470349807212516159332678,
    0.0509980592623761761961632446895216952601847767397628437069071236525030510385137821267442193868358292147899714519363571211100873456269865150186456681043804358654826791768545393024953758025593924464295555854744882720755747096079325496814455853004350452095212995888025282619932613606999567133,
    0.0509980592623761761961632446895216952601847767397628437069071236525030510385137821267442193868358292147899714519363571211100873456269865150186456681043804358654826791768545393024953758025593924464295555854744882720755747096079325496814455853004350452095212995888025282619932613606999567133,
    0.0428358980222266806568786466061255284928108575989407395620219408911043916962572261359138025961596979511472539467367407419206021900868371610612953162236233351132214438513203223655531564777278515080476421262443325932320214191168239648611793958596884827086182431203349730049744697408543115307,
    0.0428358980222266806568786466061255284928108575989407395620219408911043916962572261359138025961596979511472539467367407419206021900868371610612953162236233351132214438513203223655531564777278515080476421262443325932320214191168239648611793958596884827086182431203349730049744697408543115307,
    0.0342738629130214331026877322523727069948402029116274337814057454192310522168984446294442724624445760666244242305266023810860790282088335398182296698622433517061843276344829146573593201201081743714879684153735672789104567624853712011151505225193933019375481618760594889854480408562043658635,
    0.0342738629130214331026877322523727069948402029116274337814057454192310522168984446294442724624445760666244242305266023810860790282088335398182296698622433517061843276344829146573593201201081743714879684153735672789104567624853712011151505225193933019375481618760594889854480408562043658635,
    0.0253920653092620594557525897892240292875540475469487209362512822192154788532376645960457016338988332029324531233401833547954942765653767672102838323550828207273795044402516181251040411735351747299230615776597356956641506445501689924551185923348003766988424170511157069264716719906995309826,
    0.0253920653092620594557525897892240292875540475469487209362512822192154788532376645960457016338988332029324531233401833547954942765653767672102838323550828207273795044402516181251040411735351747299230615776597356956641506445501689924551185923348003766988424170511157069264716719906995309826,
    0.0162743947309056706051705622063866181795429637952095664295931749613369651752917857651844425586692833071042366002861684552859449530958901379260437604156888337987656773068694383447504913457771896770689760342192010638946676879735404121702279005140285599424477022083127753774756520463311689155,
    0.0162743947309056706051705622063866181795429637952095664295931749613369651752917857651844425586692833071042366002861684552859449530958901379260437604156888337987656773068694383447504913457771896770689760342192010638946676879735404121702279005140285599424477022083127753774756520463311689155,
    0.0070186100094700966004070637388531825133772207289396032320082356192151241454178686953297376907573215077936155545790593837513204206518026084505878987243348925784479817181234617862457418214505322067610482902501455504204433524520665822704844582452877416001060465891907497519632353148380799619,
    0.0070186100094700966004070637388531825133772207289396032320082356192151241454178686953297376907573215077936155545790593837513204206518026084505878987243348925784479817181234617862457418214505322067610482902501455504204433524520665822704844582452877416001060465891907497519632353148380799619
            };

            var arguments = new double[]
            {
                -0.0483076656877383162348125704405021636908472517308488971677937345463685926042778777794060365911173780988289503411375793689757446357461295741679964108035347980667582792392651327368009453047606446744575790523465655622949909588624860214137051585425884056992683442137333250625173849291299678673,
    0.0483076656877383162348125704405021636908472517308488971677937345463685926042778777794060365911173780988289503411375793689757446357461295741679964108035347980667582792392651327368009453047606446744575790523465655622949909588624860214137051585425884056992683442137333250625173849291299678673,
    -0.1444719615827964934851863735988106522038459913156355521379528938242184438164519731102406769974924713989580220758441301598578946580142268413547299935841673092513202403499286272686350814272974392746706128556678811982653393383080797337231702069432462445053984587997153683967433095128570624414,
    0.1444719615827964934851863735988106522038459913156355521379528938242184438164519731102406769974924713989580220758441301598578946580142268413547299935841673092513202403499286272686350814272974392746706128556678811982653393383080797337231702069432462445053984587997153683967433095128570624414,
    -0.2392873622521370745446032091655015206088554219602530155470960995597029133039943915553593695844147813728958071901224632260145752503694970545640339873418480550362677768010887468668377893757173424222709744116861683634989914911762187599464033126988486345234374380695224452457957624756811128321,
    0.2392873622521370745446032091655015206088554219602530155470960995597029133039943915553593695844147813728958071901224632260145752503694970545640339873418480550362677768010887468668377893757173424222709744116861683634989914911762187599464033126988486345234374380695224452457957624756811128321,
    -0.3318686022821276497799168057301879961957751368050598360182296306285376829657438169809731852312743263005943551508559377834274303920771100489026913715847854727626540340157368609696698131829681988642689780208633461925468064919389286805624602715005948661328152252049795463242055567997437182143,
    0.3318686022821276497799168057301879961957751368050598360182296306285376829657438169809731852312743263005943551508559377834274303920771100489026913715847854727626540340157368609696698131829681988642689780208633461925468064919389286805624602715005948661328152252049795463242055567997437182143,
    -0.4213512761306353453641194361724264783358772886324433305416613404557190462549837315607633055675740638739884093394574651160978879545562247406839036854173715776910866941643197988581928900702286425821151586000969947406313405310082646561917980302543820974679501841964453794193724645925031841919,
    0.4213512761306353453641194361724264783358772886324433305416613404557190462549837315607633055675740638739884093394574651160978879545562247406839036854173715776910866941643197988581928900702286425821151586000969947406313405310082646561917980302543820974679501841964453794193724645925031841919,
    -0.5068999089322293900237474743778212301802836995994354639743662809707712640478764442266190213124522047999876916596854537447047905434649918210338296049592120273725464263651562560829050004258268002241145951271730860506703690843719936432852920782304931272053564539127514959875734718036950073563,
    0.5068999089322293900237474743778212301802836995994354639743662809707712640478764442266190213124522047999876916596854537447047905434649918210338296049592120273725464263651562560829050004258268002241145951271730860506703690843719936432852920782304931272053564539127514959875734718036950073563,
    -0.5877157572407623290407454764018268584509401154544205727031788473129228586684474311408145102018661764979429510790747919023774933113319119601088669936958908618326367715806216053155906936017362413244183150445492317940727345571648726363597097311647731726438279098059670236086983675374932643925,
    0.5877157572407623290407454764018268584509401154544205727031788473129228586684474311408145102018661764979429510790747919023774933113319119601088669936958908618326367715806216053155906936017362413244183150445492317940727345571648726363597097311647731726438279098059670236086983675374932643925,
    -0.6630442669302152009751151686632383689770222859605053010170834964924461749232229404368981536611965356686820332804126742949900731319113817214392193185613161549689934301410316417342588149871686184296988807305719690974644891055567340650986465615021143958920599684258616066247948224049997371166,
    0.6630442669302152009751151686632383689770222859605053010170834964924461749232229404368981536611965356686820332804126742949900731319113817214392193185613161549689934301410316417342588149871686184296988807305719690974644891055567340650986465615021143958920599684258616066247948224049997371166,
    -0.732182118740289680387426665091267146630270483506629100821139573270385253587797727611292298988652560055905228466313310601075333829094630570926240639601009902567982815376254840388565733846030450161774620971196087756484387383432502715118096615117242484073636640563609696801484680439912327302,
    0.732182118740289680387426665091267146630270483506629100821139573270385253587797727611292298988652560055905228466313310601075333829094630570926240639601009902567982815376254840388565733846030450161774620971196087756484387383432502715118096615117242484073636640563609696801484680439912327302,
    -0.7944837959679424069630972989704289020954794016388354532507582449720593922816426654241878967890821228397041480126630294067578180914548706957761322921470535094589673860419616615738928385807346185892317514562489971543238450942224396667500582904031225063621511429185567036727089257387570529468,
    0.7944837959679424069630972989704289020954794016388354532507582449720593922816426654241878967890821228397041480126630294067578180914548706957761322921470535094589673860419616615738928385807346185892317514562489971543238450942224396667500582904031225063621511429185567036727089257387570529468,
    -0.849367613732569970133693004967742538954886793049759233100219598613724656141562558741881463752754991143937635778596582088915769685796612254240615386941355933272723068952531445772190363422003834495043219316062885999846179078139659341918527603834809670576387535564876596379488780285979062125,
    0.849367613732569970133693004967742538954886793049759233100219598613724656141562558741881463752754991143937635778596582088915769685796612254240615386941355933272723068952531445772190363422003834495043219316062885999846179078139659341918527603834809670576387535564876596379488780285979062125,
    -0.8963211557660521239653072437192122684789964967957595765636154129650249794910409173494503783167666654202705333374285522819507600044591355080910768854012859468015827508424619812224062460791781333400979810176198916239783226706506012473250929962326307746466256167673927887144428859779028909399,
    0.8963211557660521239653072437192122684789964967957595765636154129650249794910409173494503783167666654202705333374285522819507600044591355080910768854012859468015827508424619812224062460791781333400979810176198916239783226706506012473250929962326307746466256167673927887144428859779028909399,
    -0.9349060759377396891709191348354093255286714322828372184584037398118161947182932855418880831417927728359606280450921427988850058691931014887248988124656348299653052688344696135840215712191162135178273756415771123010111796122671724143565383396162107206772781551029308751511942924942333859805,
    0.9349060759377396891709191348354093255286714322828372184584037398118161947182932855418880831417927728359606280450921427988850058691931014887248988124656348299653052688344696135840215712191162135178273756415771123010111796122671724143565383396162107206772781551029308751511942924942333859805,
    -0.9647622555875064307738119281182749603888952204430187193220113218370995254867038008243801877562227002840740910741483519987441236283464394249183812395373150090695515823078220949436846111682404866338388944248976976566275875721000356873959697266702651250019105084704924793016185368873243713355,
    0.9647622555875064307738119281182749603888952204430187193220113218370995254867038008243801877562227002840740910741483519987441236283464394249183812395373150090695515823078220949436846111682404866338388944248976976566275875721000356873959697266702651250019105084704924793016185368873243713355,
    -0.9856115115452683354001750446309019786323957143358063182107821705820305847193755946663846485510970266115353839862364606643634021712823093784875255943834038377710426488328772047833289470320023596895438028281274741367781028592272459887917924171204666683239464005128153533797603112851826904814,
    0.9856115115452683354001750446309019786323957143358063182107821705820305847193755946663846485510970266115353839862364606643634021712823093784875255943834038377710426488328772047833289470320023596895438028281274741367781028592272459887917924171204666683239464005128153533797603112851826904814,
    -0.9972638618494815635449811286650407271385376637294611593011185457862359083917418520130456693085426416474280482200936551645510686196373231416035137741332968299789863385253514914078766236061488136738023162574655835389902337937054326098485227311719825229066712510246574949376367552421728646398,
    0.9972638618494815635449811286650407271385376637294611593011185457862359083917418520130456693085426416474280482200936551645510686196373231416035137741332968299789863385253514914078766236061488136738023162574655835389902337937054326098485227311719825229066712510246574949376367552421728646398
            };

            double integralValue = default;

            for (var i = 0; i < n; i++)
            {
                var transformedArg = (b + a) / 2 + ((b - a) / 2) * arguments[i];
                integralValue += weights[i] * function(transformedArg);
            }

            integralValue = ((b - a) / 2) * integralValue;

            return integralValue;
        }
    }
}
