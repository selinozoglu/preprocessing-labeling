using ExcelDataReader;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using net.zemberek.erisim;
using net.zemberek.tr.yapi;
using System.Windows.Input;

namespace Etiketleme
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static class StopwordTool
        {
            static Dictionary<string, bool> _stops = new Dictionary<string, bool>
        {
            { "a", true },{ "aa", true },{ "ise", true },{ "ama", true },{ "abe", true },
            { "abes", true },{ "abo", true },{ "acaba", true },{ "acayip", true },{ "acele", true },
            { "aceleten", true },{ "acep", true },{ "acımasız", true },{ "acımasızcasına", true },{ "acilen", true },
            { "âciz", true },{ "âcizane", true },{ "aç", true },{ "açık", true },{ "açıkçası", true },{ "açıktan", true },
            { "adamakıllı", true },{ "adamcasına", true },{ "adedî", true },{ "âdeta", true },{ "adına", true },{ "adilane", true },
            { "afedersin", true },{ "aferin", true },{ "agucuk", true },{ "ağababa", true },{ "ağabey", true },{ "ağır", true },
            { "ağızdan", true },{ "ah", true },{ "aha", true },{ "ahacık", true },{ "ahbap", true },{ "aheste", true },
            { "ahir", true },{ "ahiren", true },{ "ahlaken", true },{ "ailecek", true },{ "ait", true },{ "akabinde", true },
            { "akıbet", true },{ "altı", true },{ "altmış", true },{ "ancak", true },{ "arada", true },{ "artık", true },
            { "asla", true },{ "aslında", true },{ "ayrıca", true },{ "az", true },{ "bana", true },{ "bazen", true },{ "bazı", true },
            { "bazıları", true },{ "belki", true },{ "ben", true },{ "benden", true },{ "beni", true },{ "beri", true },{ "beş", true },
            { "bile", true },{ "bilhassa", true },{ "bin", true },{ "bir", true },{ "biraz", true },{ "birçoğu", true },{ "birçok", true },
            { "biri", true },{ "birisi", true },{ "birkaç", true },{ "birşey", true },{ "biz", true },{ "bizden", true },{ "bize", true },
            { "bizi", true },{ "bizim", true },{ "böyle", true },{ "böylece", true },{ "bu", true },{ "buna", true },{ "bunda", true },
            { "bundan", true },{ "bunlar", true },{ "bunları", true },{ "bunların", true },{ "bunu", true },{ "bunun", true },{ "burada", true },
            { "bütün", true },{ "çoğu", true },{ "çoğunu", true },{ "çok", true },{ "çünkü", true },{ "da", true },{ "daha", true },
            { "dahi", true },{ "dan", true },{ "de", true },{ "defa", true },{ "değil", true },{ "diğer", true },{ "diğeri", true },
            { "diğerleri", true },{ "diye", true },{ "doksan", true },{ "dokuz", true },{ "dolayı", true },{ "dolayısıyla", true },
            { "dört", true },{ "edecek", true },{ "eden", true },{ "ederek", true },{ "edilecek", true },{ "ediliyor", true },{ "edilmesi", true },
            { "ediyor", true },{ "eğer", true },{ "elbette", true },{ "elli", true },{ "en", true },{ "etmesi", true },{ "etti", true },{ "ettiği", true },
            { "ettiğini", true },{ "fakat", true },{ "falan", true },{ "filan", true },{ "gene", true },{ "gereği", true },{ "gerek", true },
            { "gibi", true },{ "göre", true },{ "hala", true },{ "halde", true },{ "halen", true },{ "hangi", true },{ "hangisi", true },
            { "hani", true },{ "hatta", true },{ "hem", true },{ "henüz", true },{ "hep", true },{ "hepsi", true },{ "her", true },
            { "herhangi", true },{ "herkes", true },{ "herkese", true },{ "herkesi", true },{ "herkesin", true },{ "hiç", true },
            { "hiçbir", true },{ "hiçbiri", true },{ "için", true },{ "içinde", true },{ "iki", true },{ "ile", true },{ "ilgili", true },
            { "işte", true },{ "itibaren", true },{ "itibariyle", true },{ "kaç", true },{ "kadar", true },
            { "karşın", true },{ "kendi", true },{ "kendilerine", true },{ "kendine", true },{ "kendini", true },{ "kendisi", true },
            { "kendisine", true },{ "kez", true },{ "ki", true },{ "kim", true },{ "kime", true },{ "kimi", true },{ "kimin", true },
            { "kimisi", true },{ "kimse", true },{ "kırk", true },{ "madem", true },{ "mi", true },{ "mı", true },{ "milyar", true },
            { "milyon", true },{ "mu", true },{ "mü", true },{ "nasıl", true },{ "ne", true },{ "neden", true },{ "nedenle", true },
            { "nerede", true },{ "nerde", true },{ "nereye", true },{ "neyse", true },{ "niçin", true },{ "nin", true },{ "nın", true },
            { "niye", true },{ "nun", true },{ "nün", true },{ "o", true },{ "öbür", true },{ "olan", true },{ "olarak", true },
            { "oldu", true },{ "olduğu", true },{ "olduğunu", true },{ "olduklarını", true },{ "olmadı", true },{ "olmadığı", true },
            { "olmak", true },{ "olması", true },{ "olmayan", true },{ "olmaz", true },{ "olsa", true },{ "olsun", true },
            { "olup", true },{ "olur", true }, {"olursa", true },{ "oluyor", true },{ "on", true },{ "ön", true },{ "ona", true },{ "önce", true },
            { "ondan", true },{ "onlar", true },{ "onlara", true },{ "onlardan", true },{ "onları", true },{ "onların", true },
            { "onu", true },{ "onun", true },{ "orada", true },{ "öte", true },{ "ötürü", true },{ "otuz", true },{ "öyle", true },
            { "oysa", true },{ "pek", true },{ "rağmen", true },{ "sana", true },{ "sanki", true },{ "şayet", true },{ "şekilde", true },
            { "sekiz", true },{ "seksen", true },{ "sen", true },{ "senden", true },{ "seni", true },{ "senin", true },{ "şey", true },
            { "şeyden", true },{ "şeye", true },{ "şeyi", true },{ "şeyler", true },{ "şimdi", true },{ "siz", true },
            { "sizden", true },{ "size", true },{ "sizi", true },{ "sizin", true },{ "sonra", true },{ "şöyle", true },
            { "şu", true },{ "şuna", true },{ "şunları", true },{ "şunu", true },{ "ta", true },{ "tabi", true },{ "tam", true },
            { "tamam", true },{ "tamamen", true },{ "tarafından", true },{ "tüm", true },{ "trilyon", true },{ "tümü", true },
            { "üç", true },{ "un", true },{ "ün", true },{ "üzeri", true },{ "var", true },{ "vardı", true },{ "ve", true },{ "veya", true },
            { "ya", true },{ "yani", true },{ "yapacak", true },{ "yapılan", true },{ "yapılması", true },{ "yapıyor", true },
            { "yapmak", true },{ "yaptı", true },{ "yaptığı", true },{ "yaptığını", true },{ "yaptıkları", true },{ "ye", true },
            { "yedi", true },{ "yerine", true },{ "yı", true },{ "yi", true },{ "yetmiş", true },{ "yine", true },
            { "yirmi", true },{ "yoksa", true },{ "yu", true },{ "yü", true },{ "zaten", true },{ "zira", true }
            };

            static char[] _delimiters = new char[]
            {
                ' ',
                ',',
                ';',
                '.'
            };
            public static string RemoveStopwords(string input)
            {
                var words = input.Split(_delimiters,
                    StringSplitOptions.RemoveEmptyEntries);
                var found = new Dictionary<string, bool>();
                StringBuilder builder = new StringBuilder();
                foreach (string currentWord in words)
                {
                    string lowerWord = currentWord.ToLower();
                    if (!_stops.ContainsKey(lowerWord) &&
                        !found.ContainsKey(lowerWord))
                    {
                        builder.Append(currentWord).Append(' ');
                        found.Add(lowerWord, true);
                    }
                }
                return builder.ToString().Trim();
            }
        }
        
        //tweetler = Regex.Replace(tweetler, @"[^\u0000-\u007F]+", string.Empty);//emoji silme türkçe karakterler de gidiyor düzelt
        private void rbAltin_Checked(object sender, RoutedEventArgs e)
        {
            gbAltin.Visibility = Visibility.Visible;
            if (rbAltin.IsEnabled)
            {
                rbBorsa.IsEnabled = false;
                rbDoviz.IsEnabled = false;
                rbEkonomi.IsEnabled = false;
                rbEnflasyon.IsEnabled = false;
                rbAlakasiz.IsEnabled = false;
            }
        }
        private void rbBorsa_Checked(object sender, RoutedEventArgs e)
        {
            gbBorsa.Visibility = Visibility.Visible;
            if (rbBorsa.IsEnabled)
            {
                rbAltin.IsEnabled = false;
                rbDoviz.IsEnabled = false;
                rbEkonomi.IsEnabled = false;
                rbEnflasyon.IsEnabled = false;
                rbAlakasiz.IsEnabled = false;
            }
        }
        private void rbDoviz_Checked(object sender, RoutedEventArgs e)
        {
            gbDoviz.Visibility = Visibility.Visible;
            if (rbDoviz.IsEnabled)
            {
                rbBorsa.IsEnabled = false;
                rbAltin.IsEnabled = false;
                rbEkonomi.IsEnabled = false;
                rbEnflasyon.IsEnabled = false;
                rbAlakasiz.IsEnabled = false;
            }
            if (rbDovizDolar.IsChecked == true && rbDovizEuro.IsChecked == true
                && rbDovizDiger.IsChecked == true && rbDovizAlakasiz.IsChecked == true)
            {
                gbDovizDuygu.Visibility = Visibility.Visible;
            }
        }
        private void rbEkonomi_Checked(object sender, RoutedEventArgs e)
        {
            gbEkonomi.Visibility = Visibility.Visible;
            if (rbEkonomi.IsEnabled)
            {
                rbBorsa.IsEnabled = false;
                rbDoviz.IsEnabled = false;
                rbAltin.IsEnabled = false;
                rbEnflasyon.IsEnabled = false;
                rbAlakasiz.IsEnabled = false;
            }
        }
        private void rbEnflasyon_Checked(object sender, RoutedEventArgs e)
        {
            gbEnflasyon.Visibility = Visibility.Visible;
            if (rbEnflasyon.IsEnabled)
            {
                rbBorsa.IsEnabled = false;
                rbDoviz.IsEnabled = false;
                rbEkonomi.IsEnabled = false;
                rbAltin.IsEnabled = false;
                rbAlakasiz.IsEnabled = false;
            }
        }
        private void btnGeriAl_Click(object sender, RoutedEventArgs e)
        {
            rbEnflasyon.IsEnabled = true;
            rbBorsa.IsEnabled = true;
            rbDoviz.IsEnabled = true;
            rbEkonomi.IsEnabled = true;
            rbAltin.IsEnabled = true;
            rbAlakasiz.IsEnabled = true;

            rbEnflasyon.IsChecked = false;
            rbBorsa.IsChecked = false;
            rbDoviz.IsChecked = false;
            rbEkonomi.IsChecked = false;
            rbAltin.IsChecked = false;
            rbAlakasiz.IsChecked = false;

            rbAltinOlumlu.IsChecked = false;
            rbAltinOlumsuz.IsChecked = false;
            rbAltinTarafsiz.IsChecked = false;
            rbAltinAlakasiz.IsChecked = false;

            rbBorsaOlumlu.IsChecked = false;
            rbBorsaOlumsuz.IsChecked = false;
            rbBorsaTarafsiz.IsChecked = false;
            rbBorsaAlakasiz.IsChecked = false;

            rbEkonomiOlumlu.IsChecked = false;
            rbEkonomiOlumsuz.IsChecked = false;
            rbEkonomiTarafsiz.IsChecked = false;
            rbEkonomiAlakasiz.IsChecked = false;

            rbEnflasyonOlumlu.IsChecked = false;
            rbEnflasyonOlumsuz.IsChecked = false;
            rbEnflasyonTarafsiz.IsChecked = false;
            rbEnflasyonAlakasiz.IsChecked = false;

            rbDovizDolar.IsChecked = false;
            rbDovizEuro.IsChecked = false;
            rbDovizDiger.IsChecked = false;
            rbDovizAlakasiz.IsChecked = false;
            rbDovizDuyguOlumlu.IsChecked = false;
            rbDovizDuyguOlumsuz.IsChecked = false;
            rbDovizDuyguTarafsiz.IsChecked = false;
            rbDovizDuyguAlakasiz.IsChecked = false;


            if (gbAltin.Visibility == Visibility.Visible)
            {
                gbAltin.Visibility = Visibility.Hidden;
            }
            else if (gbBorsa.Visibility == Visibility.Visible)
            {
                gbBorsa.Visibility = Visibility.Hidden;
            }
            else if (gbDoviz.Visibility == Visibility.Visible)
            {
                gbDoviz.Visibility = Visibility.Hidden;
                gbDovizDuygu.Visibility = Visibility.Hidden;
            }
            else if (gbDovizDuygu.Visibility == Visibility.Visible)
            {
                gbDovizDuygu.Visibility = Visibility.Hidden;
                gbDoviz.Visibility = Visibility.Hidden;
            }
            else if (gbEkonomi.Visibility == Visibility.Visible)
            {
                gbEkonomi.Visibility = Visibility.Hidden;
            }
            else if (gbEnflasyon.Visibility == Visibility.Visible)
            {
                gbEnflasyon.Visibility = Visibility.Hidden;
            }
        }
        private void rbDovizEuro_Checked(object sender, RoutedEventArgs e)
        {
            gbDovizDuygu.Visibility = Visibility.Visible;
        }
        private void rbDovizDolar_Checked(object sender, RoutedEventArgs e)
        {
            gbDovizDuygu.Visibility = Visibility.Visible;
        }
        private void rbDovizDiger_Checked(object sender, RoutedEventArgs e)
        {
            gbDovizDuygu.Visibility = Visibility.Visible;
        }

        private void rbDovizAlakasiz_Checked(object sender, RoutedEventArgs e)
        {
            gbDovizDuygu.Visibility = Visibility.Visible;
        }
        public void Veriler()
        {
            OleDbDataAdapter da = new OleDbDataAdapter();
        }
        public void TweetYazdir(string baglan, string Text, string konu)
        {
            OleDbConnection baglanti = new OleDbConnection(baglan);

            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("insert into [Sayfa1$] (Tweetler,Konu) values (@p1,@p2)", baglanti);

            komut.Parameters.AddWithValue("@p1", Text);
            komut.Parameters.AddWithValue("@p2", konu);
            komut.ExecuteNonQuery();
            baglanti.Close();
            Veriler();
        }
        public void TekKatmanDosyalaraYaz(string konusu)
        {
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanHamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanKucukHarfSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanKucukHarfSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanKucukHarfStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanSayiSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanKucukHarfSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanKucukHarfSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanTemizTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTemizTweet.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanSayiSembolKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanKucukHarfSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiStopWord.Text, konusu);
        }
        public void IlkKatmanDosyalaraYaz(string konusu)// Birinci katman
        {
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanHamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanKucukHarfSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanKucukHarfSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanKucukHarfStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanSayiSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanKucukHarfSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanKucukHarfSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanTemizTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTemizTweet.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanSayiSembolKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\IlkKatmanKucukHarfSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiStopWord.Text, konusu);
        }
        public void İkinciKatmanAltin(string konusu)
        {
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanHamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanKucukHarfSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanKucukHarfSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanKucukHarfStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanSayiSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanKucukHarfSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanKucukHarfSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanTemizTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTemizTweet.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanSayiSembolKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\AltinİkinciKatmanKucukHarfSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiStopWord.Text, konusu);
        }
        public void İkinciKatmanBorsa(string konusu)
        {
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanHamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanKucukHarfSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanKucukHarfSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanKucukHarfStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanSayiSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanKucukHarfSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanKucukHarfSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanTemizTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTemizTweet.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanSayiSembolKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\BorsaİkinciKatmanKucukHarfSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiStopWord.Text, konusu);
        }
        public void İkinciKatmanEkonomi(string konusu)
        {
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanHamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanKucukHarfSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanKucukHarfSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanKucukHarfStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanSayiSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanKucukHarfSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanKucukHarfSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanTemizTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTemizTweet.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanSayiSembolKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EkonomiİkinciKatmanKucukHarfSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiStopWord.Text, konusu);
        }
        public void İkinciKatmanEnflasyon(string konusu)
        {
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanHamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanKucukHarfSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanKucukHarfSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanKucukHarfStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanSayiSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanKucukHarfSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanKucukHarfSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanTemizTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTemizTweet.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanSayiSembolKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\EnflasyonİkinciKatmanKucukHarfSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiStopWord.Text, konusu);
        }
        public void İkinciKatmanDoviz(string konusu)
        {
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanHamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanKucukHarfSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanKucukHarfSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanKucukHarfStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanSayiSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanKucukHarfSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanKucukHarfSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanTemizTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTemizTweet.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanSayiSembolKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizİkinciKatmanKucukHarfSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiStopWord.Text, konusu);
        }
        public void UcuncuKatmanDoviz(string konusu)
        {
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanHamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanKucukHarfSayi.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayi.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanKucukHarfSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanKucukHarfStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanSayiSembol.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembol.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanKucukHarfSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanKucukHarfSayiSembolStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiSembolStopWord.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanTemizTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTemizTweet.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanSayiSembolKucukHarf.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbSayiSembolKucukHarf.Text, konusu);
            TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\DovizUcuncuKatmanKucukHarfSayiStopWord.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbKucukHarfSayiStopWord.Text, konusu);
        }
        private void btnTamam_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tweet dosyalara yazdırılacak. Emin misin?",
            "Son karar", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\HamTweet.xlsx;
            Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, "");

                rbAltin.Visibility = Visibility.Hidden;
                rbBorsa.Visibility = Visibility.Hidden;
                rbDoviz.Visibility = Visibility.Hidden;
                rbEkonomi.Visibility = Visibility.Hidden;
                rbEnflasyon.Visibility = Visibility.Hidden;
                rbAlakasiz.Visibility = Visibility.Hidden;
                gbAltin.Visibility = Visibility.Hidden;
                gbBorsa.Visibility = Visibility.Hidden;
                gbDoviz.Visibility = Visibility.Hidden;
                gbDovizDuygu.Visibility = Visibility.Hidden;
                gbEkonomi.Visibility = Visibility.Hidden;
                gbEnflasyon.Visibility = Visibility.Hidden;

                if (rbAltin.IsChecked == true && rbAltinOlumlu.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Altin_Olumlu");
                    IlkKatmanDosyalaraYaz("Altin");
                    İkinciKatmanAltin("Olumlu");
                }
                else if (rbAltin.IsChecked == true && rbAltinOlumsuz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Altin_Olumsuz");
                    IlkKatmanDosyalaraYaz("Altin");
                    İkinciKatmanAltin("Olumsuz");
                }
                else if (rbAltin.IsChecked == true && rbAltinTarafsiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Altin_Tarafsiz");
                    IlkKatmanDosyalaraYaz("Altin");
                    İkinciKatmanAltin("Tarafsız");
                }
                else if (rbAltin.IsChecked == true && rbAltinAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Altin_Alakasiz");
                    IlkKatmanDosyalaraYaz("Altin");
                    İkinciKatmanAltin("Alakasız");
                }
                else if (rbBorsa.IsChecked == true && rbBorsaOlumlu.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Borsa_Olumlu");
                    IlkKatmanDosyalaraYaz("Borsa");
                    İkinciKatmanBorsa("Olumlu");
                }
                else if (rbBorsa.IsChecked == true && rbBorsaOlumsuz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Borsa_Olumsuz");
                    IlkKatmanDosyalaraYaz("Borsa");
                    İkinciKatmanBorsa("Olumsuz");
                }
                else if (rbBorsa.IsChecked == true && rbBorsaTarafsiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Borsa_Tarafsiz");
                    IlkKatmanDosyalaraYaz("Borsa");
                    İkinciKatmanBorsa("Tarafsız");
                }
                else if (rbBorsa.IsChecked == true && rbBorsaAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Borsa_Alakasiz");
                    IlkKatmanDosyalaraYaz("Borsa");
                    İkinciKatmanBorsa("Alakasız");
                }
                else if (rbEkonomi.IsChecked == true && rbEkonomiOlumlu.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Ekonomi_Olumlu");
                    IlkKatmanDosyalaraYaz("Ekonomi");
                    İkinciKatmanEkonomi("Olumlu");
                }
                else if (rbEkonomi.IsChecked == true && rbEkonomiOlumsuz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Ekonomi_Olumsuz");
                    IlkKatmanDosyalaraYaz("Ekonomi");
                    İkinciKatmanEkonomi("Olumsuz");

                }
                else if (rbEkonomi.IsChecked == true && rbEkonomiTarafsiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Ekonomi_Tarafsiz");
                    IlkKatmanDosyalaraYaz("Ekonomi");
                    İkinciKatmanEkonomi("Tarafsiz");
                }
                else if (rbEkonomi.IsChecked == true && rbEkonomiAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Ekonomi_Alakasiz");
                    IlkKatmanDosyalaraYaz("Ekonomi");
                    İkinciKatmanEkonomi("Alakasiz");
                }
                if (rbEnflasyon.IsChecked == true && rbEnflasyonOlumlu.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Enflasyon_Olumlu");
                    IlkKatmanDosyalaraYaz("Enflasyon");
                    İkinciKatmanEnflasyon("Olumlu");
                }
                else if (rbEnflasyon.IsChecked == true && rbEnflasyonOlumsuz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Enflasyon_Olumsuz");
                    IlkKatmanDosyalaraYaz("Enflasyon");
                    İkinciKatmanEnflasyon("Olumsuz");
                }
                else if (rbEnflasyon.IsChecked == true && rbEnflasyonTarafsiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Enflasyon_Tarafsiz");
                    IlkKatmanDosyalaraYaz("Enflasyon");
                    İkinciKatmanEnflasyon("Tarafsız");
                }
                else if (rbEnflasyon.IsChecked == true && rbEnflasyonAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Enflasyon_Alakasiz");
                    IlkKatmanDosyalaraYaz("Enflasyon");
                    İkinciKatmanEnflasyon("Alakasız");
                }
                else if (rbDoviz.IsChecked == true && rbDovizDolar.IsChecked == true && rbDovizDuyguOlumlu.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Dolar_Olumlu");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Dolar");
                    UcuncuKatmanDoviz("Olumlu");
                }
                else if (rbDoviz.IsChecked == true && rbDovizEuro.IsChecked == true && rbDovizDuyguOlumlu.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Euro_Olumlu");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Euro");
                    UcuncuKatmanDoviz("Olumlu");

                }
                else if (rbDoviz.IsChecked == true && rbDovizDiger.IsChecked == true && rbDovizDuyguOlumlu.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Diger_Olumlu");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Diğer");
                    UcuncuKatmanDoviz("Olumlu");
                }
                else if (rbDoviz.IsChecked == true && rbDovizAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Alakasiz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Alakasız");
                    UcuncuKatmanDoviz("Alakasız");
                }
                else if (rbDoviz.IsChecked == true && rbDovizDolar.IsChecked == true && rbDovizDuyguOlumsuz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Dolar_Olumsuz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Dolar");
                    UcuncuKatmanDoviz("Olumsuz");
                }
                else if (rbDoviz.IsChecked == true && rbDovizEuro.IsChecked == true && rbDovizDuyguOlumsuz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Euro_Olumsuz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Euro");
                    UcuncuKatmanDoviz("Olumsuz");
                }
                else if (rbDoviz.IsChecked == true && rbDovizDiger.IsChecked == true && rbDovizDuyguOlumsuz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Diger_Olumsuz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Diger");
                    UcuncuKatmanDoviz("Olumsuz");
                }
                else if (rbDoviz.IsChecked == true && rbDovizDolar.IsChecked == true && rbDovizDuyguAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Dolar_Alakasiz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Dolar");
                    UcuncuKatmanDoviz("Alakasız");
                }
                else if (rbDoviz.IsChecked == true && rbDovizEuro.IsChecked == true && rbDovizDuyguAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Euro_Alakasiz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Euro");
                    UcuncuKatmanDoviz("Alakasız");
                }
                else if (rbDoviz.IsChecked == true && rbDovizDiger.IsChecked == true && rbDovizDuyguAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Diger_Alakasiz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Diger");
                    UcuncuKatmanDoviz("Alakasız");
                }
                else if (rbDoviz.IsChecked == true && rbDovizDolar.IsChecked == true && rbDovizDuyguTarafsiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Dolar_Tarafsiz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Dolar");
                    UcuncuKatmanDoviz("Tarafsız");
                }
                else if (rbDoviz.IsChecked == true && rbDovizEuro.IsChecked == true && rbDovizDuyguTarafsiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Euro_Tarafsiz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Euro");
                    UcuncuKatmanDoviz("Tarafsız");
                }
                else if (rbDoviz.IsChecked == true && rbDovizDiger.IsChecked == true && rbDovizDuyguTarafsiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Doviz_Diger_Tarafsiz");
                    IlkKatmanDosyalaraYaz("Doviz");
                    İkinciKatmanDoviz("Diger");
                    UcuncuKatmanDoviz("Tarafsız");
                }
                else if (rbAlakasiz.IsChecked == true)
                {
                    TekKatmanDosyalaraYaz("Alakasiz");
                    IlkKatmanDosyalaraYaz("Alakasiz");
                    TweetYazdir(@"Provider=Microsoft.ACE.OLEDB.12.0;
                    Data Source=C:\Users\selin\source\repos\Etiketleme\Etiketleme\bin\Debug\Dosyalar\TekKatmanAlakasiz.xlsx;
                    Extended Properties='Excel 12.0 Xml; HDR = YES';", TbTweets.Text, "Alakasiz");
                }
            }
        }
        private void btnTweetleriGoster_Click(object sender, RoutedEventArgs e)
        {
            rbAltin.Visibility = Visibility.Visible;
            rbBorsa.Visibility = Visibility.Visible;
            rbDoviz.Visibility = Visibility.Visible;
            rbEkonomi.Visibility = Visibility.Visible;
            rbEnflasyon.Visibility = Visibility.Visible;
            rbAlakasiz.Visibility = Visibility.Visible;
            rbAltin.IsEnabled = true;
            rbBorsa.IsEnabled = true;
            rbDoviz.IsEnabled = true;
            rbEkonomi.IsEnabled = true;
            rbEnflasyon.IsEnabled = true;
            rbAlakasiz.IsEnabled = true;

            LinkSil();
            KucukHarf();
            KucukHarfSayi();
            Sayi();
            Sembol();
            StopWordTb();
            KucukHarfSembol();
            KucukHarfStopWord();
            SayiSembol();
            SayiStopWord();
            SembolStopWord();
            KucukHarfSembolStopWord();
            SayiSembolStopWord();
            KucukHarfSayiSembolStopWord();
            SayiSembolKucukHarf();
            KucukHarfSayiStopWord();
            Zemberek zemberek = new Zemberek(new TurkiyeTurkcesi());
            TemizTweet(zemberek);
        }
        public int sayac = 0;
        List<string> tweetler = new List<string>();
        int current = 0;
        private void btnSonrakiTweet_Click(object sender, RoutedEventArgs e)
        {
            TbTweets.Text = tweetler[current];
            current++;
            LblBulunduguKayit.Content = "Şu an bulunan satır numarası : " + current;

            rbAltin.IsChecked = false;
            rbBorsa.IsChecked = false;
            rbDoviz.IsChecked = false;
            rbEkonomi.IsChecked = false;
            rbEnflasyon.IsChecked = false;
            rbAlakasiz.IsChecked = false;
        }
        int kayitsayisi = 0;
        string kayitsirasi = "";
        public void excelAc()
        {
            OpenFileDialog dosyaAc = new OpenFileDialog();
            dosyaAc.Title = "Dosya Seç";
            dosyaAc.Filter = "Excel Dosyası|*.xlsx";
            if (dosyaAc.ShowDialog() == true)
            {
                TbTweets.Text = dosyaAc.FileName;
            }
            using (var stream = File.Open(dosyaAc.FileName, FileMode.Open, FileAccess.Read))
            {
                using (var excelReader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet sonuc = excelReader.AsDataSet(new ExcelDataSetConfiguration());
                    //TbTweets.Text = sonuc.Tables[0].Rows[0][0].ToString();                 
                    for (int i = 0; i < sonuc.Tables[0].Rows.Count; i++)
                    {
                        tweetler.Add(sonuc.Tables[0].Rows[i][0].ToString());
                        kayitsayisi = sonuc.Tables[0].Rows.Count;
                        kayitsirasi = sonuc.Tables[0].Rows[i][0].ToString();
                    }
                }
            }
        }
        public void LinkSil()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(a, @"(@([-.\w]+))", " ");
            TbTweets.Text = Regex.Replace(a, @"((http|https)://[\w-]+(.[\w-]+)+([\w-.,@?^=%&amp;:/~+#]*[\w-@?^=%&amp;/~+#])?)", string.Empty);
            //TbTweets.Text = Regex.Replace(a, "(?![\\#])\\p{P}", " "); 
        }
        public void KucukHarf()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(a, "\\p{P}", " ");//noktalama
            TbKucukHarf.Text = a.ToLower();
        }
        public void Sayi()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(a, "\\p{P}", " ");
            TbSayi.Text = Regex.Replace(a, @"[0-9\-]", " ");
        }
        public void Sembol()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(a, @"(#([-.\w]+))", " ");//önce hastag sil
            TbSembol.Text = Regex.Replace(a, "(?![\\#])\\p{P}", " ");//sonra noktalama
        }
        public void StopWordTb()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(a, "\\p{P}", " ");
            var stopWords = StopwordTool.RemoveStopwords(a);
            var sonuc = Regex.Split(stopWords, @"\r\n|\r\n");
            foreach (string s in sonuc)
            {
                TbStopWord.Text = s;
            }
        }
        public void KucukHarfSayi()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, "\\p{P}", " ");
            a = Regex.Replace(a, @"[0-9\-]", " ");
            TbKucukHarfSayi.Text = a.ToLower();
        }
        public void KucukHarfSembol()
        {
            string a = TbTweets.Text;
            a = TbTweets.Text.ToLower();
            a = Regex.Replace(a, @"(#([-.\w]+))", " ");
            TbKucukHarfSembol.Text = Regex.Replace(a, "\\p{P}", " ");
        }
        public void KucukHarfStopWord()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, "\\p{P}", " ");
            a = a.ToLower();
            var stopWords = StopwordTool.RemoveStopwords(a);
            var sonuc = Regex.Split(stopWords, @"\r\n|\r\n");
            foreach (string s in sonuc)
            {
                TbKucukHarfStopWord.Text = s;
            }
        }
        public void SayiSembol()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, @"(#([-.\w]+))", " ");
            a = Regex.Replace(a, @"[0-9\-]", " ");
            TbSayiSembol.Text = Regex.Replace(a, "\\p{P}", " ");
        }
        public void SayiStopWord()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, @"[0-9\-]", " ");
            a = Regex.Replace(a, "\\p{P}", " ");
            var stopWords = StopwordTool.RemoveStopwords(a);
            var sonuc = Regex.Split(stopWords, @"\r\n|\r\n");
            foreach (string s in sonuc)
            {
                TbSayiStopWord.Text = s;
            }
        }
        public void SembolStopWord()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, @"(#([-.\w]+))", " ");
            a = Regex.Replace(a, "\\p{P}", " ");
            var stopWords = StopwordTool.RemoveStopwords(a);
            var sonuc = Regex.Split(stopWords, @"\r\n|\r\n");
            foreach (string s in sonuc)
            {
                TbSembolStopWord.Text = s;
            }
        }
        public void KucukHarfSembolStopWord()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, @"(#([-.\w]+))", " ");
            a = Regex.Replace(a, "\\p{P}", " ");
            a = a.ToLower();
            var stopWords = StopwordTool.RemoveStopwords(a);
            var sonuc = Regex.Split(stopWords, @"\r\n|\r\n");
            foreach (string s in sonuc)
            {
                TbKucukHarfSembolStopWord.Text = s;
            }
        }
        public void SayiSembolStopWord()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, @"(#([-.\w]+))", " ");
            a = Regex.Replace(a, "\\p{P}", " ");
            a = Regex.Replace(a, @"[0-9\-]", " ");
            var stopWords = StopwordTool.RemoveStopwords(a);
            var sonuc = Regex.Split(stopWords, @"\r\n|\r\n");
            foreach (string s in sonuc)
            {
                TbSayiSembolStopWord.Text = s;
            }
        }
        public void KucukHarfSayiSembolStopWord()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, @"(#([-.\w]+))", " ");
            a = Regex.Replace(a, "\\p{P}", " ");
            a = a.ToLower();
            a = Regex.Replace(a, @"[0-9\-]", " ");
            var stopWords = StopwordTool.RemoveStopwords(a);
            var sonuc = Regex.Split(stopWords, @"\r\n|\r\n");
            foreach (string s in sonuc)
            {
                TbKucukHarfSayiSembolStopWord.Text = s;
            }
        }
        public void SayiSembolKucukHarf()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, @"(#([-.\w]+))", " ");
            a = Regex.Replace(a, "\\p{P}", " ");
            a = Regex.Replace(a, @"[0-9\-]", " ");
            TbSayiSembolKucukHarf.Text = a.ToLower();
        }
        public void KucukHarfSayiStopWord()
        {
            string a = TbTweets.Text;
            a = Regex.Replace(TbTweets.Text, "\\p{P}", " ");
            a = a.ToLower();
            a = Regex.Replace(a, @"[0-9\-]", " ");
            var stopWords = StopwordTool.RemoveStopwords(a);
            var sonuc = Regex.Split(stopWords, @"\r\n|\r\n");
            foreach (string s in sonuc)
            {
                TbKucukHarfSayiStopWord.Text = s;
            }
        }
        public void TemizTweet(Zemberek zemberek)
        {
            TbTemizTweet.Clear();
            string[] word = TbTweets.Text.Split(new char[] { ',', ' ', '.', '!', '-' });

            for (int i = 0; i < word.Length; i++)
            {
                if (zemberek.kelimeDenetle(word[i]))
                {
                    string kok = zemberek.kelimeCozumle(word[i])[0].kok().icerik();
                    TbTemizTweet.Text += " " + kok;
                }
            }
        }
        private void BtnDosyaAc_Click(object sender, RoutedEventArgs e)
        {
            TbIndex.Visibility = Visibility.Visible;
            excelAc();
            LblKayitSayisi.Content = "Kayıt Sayısı: " + kayitsayisi.ToString();
            LblBulunduguKayit.Content = "Şu an bulunan satır numarası : " + current;
        }
        private void btnOncekiTweet_Click(object sender, RoutedEventArgs e)
        {
            current--;
            TbTweets.Text = tweetler[current];
            LblBulunduguKayit.Content = "Şu an bulunan satır numarası : " + current;
            if (tweetler[current] == string.Empty)
            {
                MessageBox.Show("Son tweet");
            }
        }

        private void BtnTweetSec_Click(object sender, RoutedEventArgs e)
        {

            if (TbIndex.Text == string.Empty)
                MessageBox.Show("değer gir");

            current = Int32.Parse(TbIndex.Text);
            if (current - 1 < 0 && current - 1 > kayitsayisi)
            {
                MessageBox.Show("doğru sayı gir");
            }
            else
            {
                TbTweets.Text = tweetler[current - 1];
                LblBulunduguKayit.Content = "Şu an bulunan satır numarası : " + current;
            }
            rbAltin.Visibility = Visibility.Hidden;
            rbBorsa.Visibility = Visibility.Hidden;
            rbDoviz.Visibility = Visibility.Hidden;
            rbEkonomi.Visibility = Visibility.Hidden;
            rbEnflasyon.Visibility = Visibility.Hidden;
            rbAlakasiz.Visibility = Visibility.Hidden;
            gbAltin.Visibility = Visibility.Hidden;
            gbBorsa.Visibility = Visibility.Hidden;
            gbDoviz.Visibility = Visibility.Hidden;
            gbDovizDuygu.Visibility = Visibility.Hidden;
            gbEkonomi.Visibility = Visibility.Hidden;
            gbEnflasyon.Visibility = Visibility.Hidden;
        }
        private void TbIndex_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !("D1D2D3D4D5D6D7D8D9D0".Contains(e.Key.ToString()));
        }
    }
}
