using System;   // Temel veri tipleri, istisna yönetimi ve genel yardımcı metodları içeren .NET Framework’ün çekirdek işlevselliğini sağlar.
using System.Collections.Generic; // Dinamik veri yapıları (List, Dictionary gibi) ve jenerik koleksiyonları kullanabilmenizi sağlar.
// Dinamik veri yapıları (eleman sayısı çalışma zamanında değişir, örn: List<T>) ve
// jenerik koleksiyonlar (derleme zamanında tür güvenliği sağlayan koleksiyonlar, örn: List<int>, Dictionary<string, int>) kullanabilmenizi sağlar.
using System.Drawing;  // Grafiksel işlemler ve çizim işlemleri (renkler, kalemler, resimler vs.) için gerekli sınıfları içerir.
using System.Linq;  // Koleksiyonlar üzerinde sorgulama, filtreleme ve sıralama gibi işlemleri gerçekleştirmek için LINQ desteği sağlar.
// LINQ (Language Integrated Query), C# içine entegre edilmiş sorgulama dilidir.
// LINQ sayesinde, koleksiyonlar, diziler, XML, veritabanları gibi veri kaynakları üzerinde SQL benzeri sorgular yazabiliriz.
// Sorgular derleme zamanında kontrol edildiği için, hata oranı azalır ve kod daha okunabilir hale gelir.
using System.Windows.Forms;// Windows tabanlı masaüstü uygulamalarında kullanıcı arayüzü oluşturmak ve yönetmek için kullanılan temel bileşenleri sunar.


namespace WindowsFormsApp1 // Uygulamanın içinde yer alan tüm sınıfları, yapıları ve diğer üyeleri gruplamak için kullanılan ad alanıdır.
 // Bu şekilde, farklı bölümlerdeki kodlar birbirleriyle çakışmadan organize edilebilir ve yönetilebilir.
{
    // Form1 sınıfı, uygulamanın ana formunu temsil eder.
    // Bu formda 8-Taş Bulmacası'nın A* algoritması ile çözümü gerçekleştirilir.
    public partial class Form1 : Form
    {
        // *** GLOBAL DEĞİŞKENLER
        // GLOBAL DEĞİŞKENLER: Sınıf genelinde tanımlanan ve tüm metotlardan erişilebilen değişkenlerdir.
        // Bu değişkenler, uygulamanın durum bilgilerini saklamak ve metotlar arasında veri paylaşımını sağlamak için kullanılır.

        // Başlangıç durumunu saklayan 3x3 matris; 0 değeri boşluğu temsil eder.
        private int[,] baslangicDurum = new int[3, 3];
        // Hedef durumu saklayan 3x3 matris; çözüm istenen son durum.
        private int[,] hedefDurum = new int[3, 3];
        // A* algoritması sonucunda elde edilen çözüm yolunu tutacak liste.
        private List<Dugum> yol = new List<Dugum>(); // düğüm çözüm 
        // Görsel sunum için kullanılabilecek 3x3 buton dizisi (isteğe bağlı genişletilebilir).
        private Button[,] butonlar = new Button[3, 3];

        // *** YAPICI METOT (CONSTRUCTOR): Sınıfın nesnesi oluşturulurken otomatik olarak çağrılan özel metottur.
        // Bu metot, sınıfın başlangıç durumunu ayarlamak ve gerekli ilk ayarları yapmak için kullanılır.
        public Form1()
        {
            // Form özelliklerinin ayarlanması
            this.Text = "8-Taş Bulmacası A* Çözümü"; // Form başlığı
            this.Size = new Size(1000, 700);          // Formun başlangıç boyutu
            this.AutoScaleMode = AutoScaleMode.Font;  // Otomatik ölçekleme modu
            // Otomatik ölçekleme modu: Form üzerindeki kontrollerin, ekran çözünürlüğü ve DPI gibi sistem ayarlarına göre otomatik olarak boyutlandırılmasını ve düzenlenmesini sağlar.
            this.AutoScroll = true;                   // İçerik formu aştığında kaydırma çubuğu göster

            // Form üzerindeki bileşenlerin oluşturulması
            FormOlustur();
        }

        // *** FORMUN GÖRSEL YAPISININ OLUŞTURULMASI
        private void FormOlustur()
        {
            // Formun başlığı, boyutu ve scroll ayarları tekrar belirleniyor
            this.Text = "8-Taş Bulmacası A* Çözümü";
            this.Size = new Size(1000, 700);
            this.AutoScroll = true;

            // Başlangıç durumunu gösteren panelin oluşturulması
            Panel baslangicPanel = new Panel();
            baslangicPanel.Location = new Point(20, 20);    // Panelin konumu
            baslangicPanel.Size = new Size(150, 150);         // Panelin boyutu
            baslangicPanel.BorderStyle = BorderStyle.FixedSingle; // Kenarlık stili
            this.Controls.Add(baslangicPanel);   // Form üzerinde ekleme: 'baslangicPanel' adlı paneli formun kontrol koleksiyonuna ekleyerek, formda görüntülenmesini sağlar.
            

            // Hedef durumunu gösteren panelin oluşturulması
            Panel hedefPanel = new Panel();
            hedefPanel.Location = new Point(200, 20);
            hedefPanel.Size = new Size(150, 150);
            hedefPanel.BorderStyle = BorderStyle.FixedSingle; // Kenarlık stili
            this.Controls.Add(hedefPanel); // Form üzerinde ekleme

            // Başlangıç durumunu açıklayan etiketin oluşturulması, yazının oluşturulması
            Label lblBaslangic = new Label();           // 'Label' türünde bir etiket nesnesi oluşturuluyor.
            // 'lblBaslangic', yeni oluşturulan Label nesnesine referans veren değişkendir.
            // Bu sayede Label nesnesinin özelliklerini (Text, Location vb.) düzenleyebilir ve formda kullanabiliriz.
            // 'Label nesnesine referans veren değişken' ifadesi:
            // 1) 'lblBaslangic', bellekte 'new Label()' ifadesiyle oluşturulan bir Label nesnesini işaret eden değişkendir.
            // 2) Bu sayede, 'lblBaslangic' üzerinden Label nesnesinin Text, Location gibi özelliklerine ya da metodlarına erişebiliriz.
            // 3) Nesne yönelimli programlama yaklaşımında, bu tür değişkenler nesnenin bellekteki konumunu saklayarak
            // kod içinde o nesneyle etkileşime geçmemize olanak tanır.

            lblBaslangic.Text = "Başlangıç Durumu";
            lblBaslangic.Text = "Başlangıç Durumu";     // Etikete görüntülenecek metin atanıyor.
            lblBaslangic.Location = new Point(20, 180); // Etiketin form üzerindeki konumu (x=20, y=180) olarak ayarlanıyor.
            lblBaslangic.AutoSize = true;               // Etiketin metin uzunluğuna göre otomatik boyutlandırılmasını sağlıyor.
            this.Controls.Add(lblBaslangic);            // Oluşturulan etiketi formun kontrol koleksiyonuna ekleyerek ekranda görünmesini sağlıyor.


            // Hedef durumunu açıklayan etiketin oluşturulması
            Label lblHedef = new Label();
            lblHedef.Text = "Hedef Durumu";
            lblHedef.Location = new Point(200, 180);
            lblHedef.AutoSize = true;
            this.Controls.Add(lblHedef); // Oluşturulan etiketi formun kontrol koleksiyonuna ekleyerek ekranda görünmesini sağlıyor.

            // Çözüm adımlarını listeleyecek ListBox'ın oluşturulması
            ListBox lstCozumAdimlar = new ListBox();
            lstCozumAdimlar.Location = new Point(600, 20);   // ListBox'ın konumu
            lstCozumAdimlar.Size = new Size(350, 400);        // ListBox'ın boyutu
            this.Controls.Add(lstCozumAdimlar);

            // Çözümü başlatan butonun oluşturulması
            Button btnCoz = new Button();
            btnCoz.Text = "A* ile Çöz";
            btnCoz.Location = new Point(20, 220);
            btnCoz.Size = new Size(150, 40);
            btnCoz.Click += new EventHandler(BtnCoz_Click);  // Buton tıklandığında ilgili metot çalışır
            this.Controls.Add(btnCoz);

            // 3x3 butonların oluşturulması için çift döngü kullanılır (i ve j 0'dan 2'ye kadar).
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button btn = new Button();                            // Her döngüde yeni bir Button nesnesi oluşturulur.
                    btn.Size = new Size(45, 45);                          // Butonun boyutu 45x45 piksel olarak ayarlanır.
                    btn.Location = new Point(5 + j * 50, 5 + i * 50);      // Panel içindeki konumu hesaplanır (satır ve sütun düzeninde).
                    btn.Tag = new Point(i, j);                            // Butonun hangi satır ve sütunda olduğunu 'Tag' özelliğinde saklar.
                    btn.Click += new EventHandler(BaslangicButon_Click);  // Butona tıklanma olayı eklendi (BaslangicButon_Click metodu çağrılacak).
                    baslangicPanel.Controls.Add(btn);                     // Oluşturulan buton, başlangıç paneline eklenir.

                    // Aşağıda, i ve j değerlerine göre başlangıç durumunun örnek sayısal yerleşimi belirlenir.
                    // Bu kısım, başlangıç durumunun (8-Taş Bulmacası'ndaki ilk yerleşimin) elle belirlenmesidir.
                    // i ve j değerlerine göre farklı sayılar atanır, böylece 3x3 matris içinde
                    // 4, 1, 3, 2, 8, 5, 7, 0 (boşluk) ve 6 gibi bir düzen oluşur.
                    // Örneğin, i=0 ve j=0 konumunda '4' değeri; i=2 ve j=1 konumunda '0' değeri (boş hücre) vb.
                    // Bu değerler, butonun üzerinde 'Text' özelliği olarak görüntülenir.
                    // Dolayısıyla ekranda 3x3 butonlar, her biri ilgili sayıyı (veya boşluk için "") gösterir.
                    // Ortaya çıkan görünüm, 8-Taş Bulmacası için başlangıç konumunu temsil eder.
                    // 'i' ve 'j', 3x3 matrisin satır (row) ve sütun (column) indeksleridir.
                    // for (int i = 0; i < 3; i++) ve for (int j = 0; j < 3; j++) döngüleri sayesinde
                    // her satır ve sütun için bir tur döner; böylece 3x3 butonların oluşturulması sağlanır.
                    // Burada i ve j, 3x3 matrisin satır ve sütun indekslerini temsil ediyor.
                    // i değeri satırları (yukarıdan aşağıya), j değeri ise sütunları (soldan sağa) kontrol eder.
                    // yani i ve j şeklinde 2 değer olmasının sebebi satır ve sütunu temsil etmesi. 
                    // for (int i = 0; i < 3; i++) döngüsü 3 satır için çalışır,
                    // içindeki for (int j = 0; j < 3; j++) döngüsü de her satırda 3 sütun için çalışır.
                    // Böylece 3x3’lük bir düzen (toplam 9 hücre) oluşturulur. Her hücrede buton ve değer ataması yapılır.

                    // Resimdeki başlangıç durumunu ayarlıyoruz
                    int[,] yeniBaslangic = {
    { 4, 1, 3 },
    { 2, 8, 5 },
    { 7, 0, 6 }
};
                    int deger = yeniBaslangic[i, j];


                    // 'baslangicDurum' matrisine ilgili hücrenin değerini yazar ve butonun üzerinde gösterir.
                    baslangicDurum[i, j] = deger;
                    // 'baslangicDurum' 3x3 boyutunda bir dizi (matris) olup, bulmacanın başlangıç konumundaki sayıları saklar.
                    // 'baslangicDurum[i, j] = deger;' ifadesi, o dizinin i'nci satır ve j'nci sütununa 'deger' adlı sayıyı yerleştirir.

                    btn.Text = deger == 0 ? "" : deger.ToString();  // '0' ise buton metni boş, değilse sayı yazılır.
                    // Bu fonksiyonun genel işleyiş felsefesi:
                    // 1) 3x3 boyutunda iki katmanlı 'for' döngüsü, her satır (i) ve sütun (j) için bir 'Button' nesnesi oluşturur.
                    // 2) Her butonun boyutu, konumu ve tıklama olayı (Click) ayarlanır, ardından 'baslangicPanel' içine eklenir.
                    // 3) 'if-else' yapısı kullanılarak, 8-Taş Bulmacası’nın başlangıç konumundaki sayılar (ve boşluk) atanır.
                    // 4) Bu sayılar, hem 'baslangicDurum' dizisinde saklanır hem de butonun 'Text' özelliği aracılığıyla ekranda gösterilir.
                    // 5) Böylece ekranda 3x3 düzeninde, her biri belirli bir sayıyı (veya boşluk) temsil eden butonlar oluşur.
                    // 6) Ortaya çıkan görünüm, bulmacanın başlangıç durumunu temsil eder ve her buton, tıklandığında ilgili işlevi tetikler.

                }
            }
            // Bu döngü tamamlandığında, panel üzerinde 3x3 butonlar oluşmuş olur ve
            // her buton başlangıç durumundaki sayısal değerine göre etiketlenir (örneğin 4, 1, 3, vb.).


            // Hedef durumu butonlarının (3x3) oluşturulması
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(45, 45);
                    btn.Location = new Point(5 + j * 50, 5 + i * 50);
                    btn.Tag = new Point(i, j);
                    btn.Click += new EventHandler(HedefButon_Click);
                    hedefPanel.Controls.Add(btn);

                    // Örnek hedef durumunun ayarlanması
                    // Resimdeki hedef durumu ayarlıyoruz
                    int[,] yeniHedef = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 0 }
};
                    int deger = yeniHedef[i, j];
                    hedefDurum[i, j] = deger;
                    btn.Text = deger == 0 ? "" : deger.ToString();
                }
            }
        }

        // *** BAŞLANGIÇ BUTONUNA TIKLANDIĞINDA ÇALIŞAN METOT
        // 'private': Bu metodun yalnızca aynı sınıf içinden erişilebileceğini belirtir.
        // 'void': Bu metodun herhangi bir değer döndürmediğini ifade eder.
        // 'void': Bu metot herhangi bir değer döndürmez; yalnızca belirli işlemleri (örneğin ekrana mesaj yazma) gerçekleştirmek için kullanılır.
        // Başka bir deyişle, işlem bittiğinde çağıran koda geri dönecek bir sonuç yoktur.

        // 'BaslangicButon_Click': Metodun ismi; kullanıcı başlangıç butonuna tıkladığında çalışır.
        // '(object sender, EventArgs e)': 'sender' olayı tetikleyen nesneyi (örneğin tıklanan buton), 
        // 'e' ise olaya ait ek bilgileri (event data) taşır.
        // Bu metod, butona tıklandığında hangi işlemlerin yapılacağını tanımlar.

        private void BaslangicButon_Click(object sender, EventArgs e)
        {
            Button tiklanilan = sender as Button;    // Tıklanan butonu elde et
            Point konum = (Point)tiklanilan.Tag;        // Butonun konum bilgisini al

            // Kullanıcıdan yeni değer almak için alternatif bir form oluşturuluyor.
            Form inputForm = new Form();
            inputForm.Width = 300;
            inputForm.Height = 150;
            inputForm.Text = "Değer Giriş";

            // Kullanıcıya bilgi veren etiket
            Label lbl = new Label();
            lbl.Text = "Yeni değeri girin (0-8 arası, 0 boş demektir):";
            lbl.Location = new Point(10, 10);
            lbl.Width = 280;

            // Mevcut değeri göstermek için TextBox
            TextBox txt = new TextBox();
            txt.Location = new Point(10, 40);
            txt.Text = baslangicDurum[konum.X, konum.Y].ToString();
            // 'ToString()': Sayısal bir değeri veya herhangi bir nesneyi metinsel (string) hale dönüştürür.
            // Bu sayede 'txt.Text' özelliğine atanarak ekranda metin olarak görüntülenebilir.

            // Giriş onay butonu
            Button btnOk = new Button();
            btnOk.Text = "Tamam";
            btnOk.Location = new Point(10, 70);
            btnOk.DialogResult = DialogResult.OK;

            // Form elemanlarını ekle:
            // 'lbl', 'txt' ve 'btnOk' adlı kontrolleri 'inputForm' üzerinde görüntülenmek üzere ekliyoruz.
            // 'inputForm.AcceptButton = btnOk;' ifadesiyle, kullanıcı Enter tuşuna bastığında 'btnOk' butonuna basılmış gibi davranılmasını sağlıyoruz.
            inputForm.Controls.Add(lbl);
            inputForm.Controls.Add(txt);
            inputForm.Controls.Add(btnOk);
            inputForm.AcceptButton = btnOk;


            // Kullanıcı onay verirse
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                int yeniDeger;
                if (int.TryParse(txt.Text, out yeniDeger) && yeniDeger >= 0 && yeniDeger <= 8)
                {
                    // Aynı değerin başka bir konumda olup olmadığını kontrol et
                    bool zatenVar = false;
                    for (int i = 0; i < 3 && !zatenVar; i++)
                    {
                        for (int j = 0; j < 3 && !zatenVar; j++)
                        {
                            if ((i != konum.X || j != konum.Y) && baslangicDurum[i, j] == yeniDeger)
                            {
                                zatenVar = true;
                            }
                        }
                    }

                    if (zatenVar)
                    {
                        MessageBox.Show("Bu değer zaten başka bir konumda var!", "Hata");
                        return;
                    }

                    // Yeni değeri matrise ve butonun yazısına ata
                    baslangicDurum[konum.X, konum.Y] = yeniDeger;
                    tiklanilan.Text = yeniDeger == 0 ? "" : yeniDeger.ToString();
                }
            }
        }

        // *** HEDEF BUTONUNA TIKLANDIĞINDA ÇALIŞAN METOT
        // Hedef durumundaki butonlara tıklanıldığında çalışır.
        private void HedefButon_Click(object sender, EventArgs e)
        {
            // 1) Tıklanan butonu, 'sender' nesnesinden 'Button' tipine dönüştürür.
            Button tiklanilan = sender as Button;

            // 2) Tıklanan butonun Tag özelliğinde saklanan (i, j) konum bilgisini alır.
            //    Bu konum, hedefDurum matrisinde hangi satır ve sütunu değiştireceğimizi belirler.
            Point konum = (Point)tiklanilan.Tag;

            // 3) Kullanıcıdan yeni değer almak için geçici bir form oluşturulur.
            //    Bu form, küçük bir pencere şeklinde açılır ve kullanıcı buradan değer girer.
            Form inputForm = new Form();
            inputForm.Width = 300;
            inputForm.Height = 150;
            inputForm.Text = "Değer Giriş";

            // 4) Kullanıcıya ne yapması gerektiğini anlatan etiket oluşturulur ve konumlandırılır.
            Label lbl = new Label();
            lbl.Text = "Yeni değeri girin (0-8 arası, 0 boş demektir):";
            lbl.Location = new Point(10, 10);
            lbl.Width = 280;

            // 5) Kullanıcının girdiği metni alacağımız TextBox oluşturulur.
            //    Varsayılan olarak, mevcut hücrenin değerini (hedefDurum[konum.X, konum.Y]) gösterir.
            TextBox txt = new TextBox();
            txt.Location = new Point(10, 40);
            txt.Text = hedefDurum[konum.X, konum.Y].ToString();

            // 6) 'Tamam' butonu oluşturulur. Kullanıcı bu butona tıkladığında
            //    formdan DialogResult.OK değeri döner.
            Button btnOk = new Button();
            btnOk.Text = "Tamam";
            btnOk.Location = new Point(10, 70);
            btnOk.DialogResult = DialogResult.OK;

            // 7) Oluşturulan kontroller (lbl, txt, btnOk) inputForm’a eklenir.
            inputForm.Controls.Add(lbl);
            inputForm.Controls.Add(txt);
            inputForm.Controls.Add(btnOk);

            // 8) 'Enter' tuşuna basıldığında 'btnOk' butonunun tıklanmış gibi davranmasını sağlar.
            inputForm.AcceptButton = btnOk;

            // 9) inputForm.ShowDialog(), bu formu diyalog penceresi olarak açar.
            //    Eğer kullanıcı 'Tamam' butonuna tıklarsa, DialogResult.OK döner.
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                // 10) Kullanıcının girdiği metni tam sayıya dönüştürmeye çalışır.
                //     'out yeniDeger' ile elde edilen değer yeniDeger değişkenine yazılır.
                int yeniDeger;
                if (int.TryParse(txt.Text, out yeniDeger) && yeniDeger >= 0 && yeniDeger <= 8)
                {
                    // 11) Yeni girilen değerin (yeniDeger), hedefDurum dizisinde zaten olup olmadığını kontrol eder.
                    //     Eğer aynı değer başka bir hücrede varsa, kullanıcıya hata mesajı gösterilir.
                    bool zatenVar = false;
                    for (int i = 0; i < 3 && !zatenVar; i++)
                    {
                        for (int j = 0; j < 3 && !zatenVar; j++)
                        {
                            // Aynı değeri farklı bir hücrede bulursa zatenVar = true yapılır.
                            if ((i != konum.X || j != konum.Y) && hedefDurum[i, j] == yeniDeger)
                            {
                                zatenVar = true;
                            }
                        }
                    }

                    // 12) Eğer değer başka bir hücrede varsa, uyarı verir ve metottan çıkar.
                    if (zatenVar)
                    {
                        MessageBox.Show("Bu değer zaten başka bir konumda var!", "Hata");
                        return;
                    }

                    // 13) Yeni değeri, hedefDurum matrisinin ilgili (konum.X, konum.Y) hücresine yazar.
                    hedefDurum[konum.X, konum.Y] = yeniDeger;

                    // 14) Tıklanan butonun üzerindeki metni de günceller. 0 ise boş, değilse sayı olarak yazılır.
                    tiklanilan.Text = yeniDeger == 0 ? "" : yeniDeger.ToString();
                }
            }
        }


        // *** ÇÖZ BUTONUNA TIKLANDIĞINDA ÇALIŞAN METOT
        // "A* ile Çöz" butonuna basıldığında A* algoritması çalıştırılarak çözüm adımları elde edilir.
        private void BtnCoz_Click(object sender, EventArgs e)
        {
            // 1) Çözüm adımlarını göstereceğimiz ListBox'ı buluyoruz.
            //    'OfType<ListBox>().FirstOrDefault()' ifadesi, formdaki ilk ListBox kontrolünü getirir.
            // 'this.Controls' içinde bulunan tüm kontrollerden ListBox tipinde olanları filtreliyoruz (OfType<ListBox>()).
            // 'FirstOrDefault()', bulunan ListBox nesnelerinin ilkini döndürür; eğer hiç yoksa 'null' döner.
            // Böylece formdaki ilk ListBox'ı 'lstCozumAdimlar' değişkenine atamış oluruz.
            ListBox lstCozumAdimlar = this.Controls.OfType<ListBox>().FirstOrDefault();


            // 2) Eğer ListBox bulunduysa, önceki içeriklerini temizliyoruz.
            // 'ListBox', içindeki öğeleri (Items) dikey bir liste halinde görüntülemeye yarayan bir kontrol türüdür.
            // Burada 'lstCozumAdimlar' adlı ListBox bulunmuşsa, önceki çözüm adımlarından kalan bilgileri temizlemek için 'Items.Clear()' çağrılır.
            // Bu sayede yeni A* çözümü çalıştığında eski verilerle karışma olmadan liste sıfırdan doldurulur.

            if (lstCozumAdimlar != null)
            {
                lstCozumAdimlar.Items.Clear();
            }

            // 3) Daha önce oluşturulmuş "adım panellerini" temizliyoruz. (Önceki çözümlerin kalıntıları olabilir)
            //    'OfType<Panel>()' ile form üzerindeki tüm panelleri bulur, 'Name' özelliği "adimPanel_" ile başlayanları kaldırır.
            // Form üzerindeki tüm kontrolleri (this.Controls) inceliyoruz.
            // 'OfType<Panel>()' ile yalnızca 'Panel' tipindeki kontrolleri seçiyoruz ve 'ToList()' ile bunları bir listeye dönüştürüyoruz.
            // Her bir paneli (ctrl) incelerken, 'Name' özelliğinin "adimPanel_" ile başlayıp başlamadığını kontrol ediyoruz.
            // Eğer panelin ismi "adimPanel_" ile başlıyorsa, bu paneli formdan kaldırıyoruz (this.Controls.Remove(ctrl)) ve bellekten temizliyoruz (ctrl.Dispose()).
            // Bu sayede, önceki çözüm adımlarının oluşturduğu paneller ekrandan ve bellekten silinmiş olur.
            foreach (Control ctrl in this.Controls.OfType<Panel>().ToList())
            {
                if (ctrl.Name != null && ctrl.Name.StartsWith("adimPanel_"))
                {
                    this.Controls.Remove(ctrl);
                    ctrl.Dispose();
                }
            }


            // 4) ListBox'a A* algoritmasının başladığı bilgisini yazdırıyoruz.
            lstCozumAdimlar.Items.Add("A* algoritması başlatılıyor...");

            // 5) A* algoritmasını çağırarak başlangıç durumundan hedef duruma giden çözüm yolunu bulmaya çalışıyoruz.
            //    'yol' değişkeni, çözüme kadar geçen tüm adımları (Dugum listesi) tutacak.
            yol = AStar(baslangicDurum, hedefDurum);

            // 6) Eğer 'yol' boşsa veya hiç eleman yoksa, çözüm bulunamadı demektir.
            if (yol == null || yol.Count == 0)
            {
                lstCozumAdimlar.Items.Add("Çözüm bulunamadı!");
                return;
            }

            // 7) Çözüm bulunduğunda, toplam kaç adım olduğunu ListBox'a yazdırıyoruz.
            //    'yol.Count - 1' ifadesi, ilk durumdan son duruma kadar kaç hamle yapıldığını gösterir.
            lstCozumAdimlar.Items.Add("Çözüm bulundu! Toplam " + (yol.Count - 1) + " adımda.");
            lstCozumAdimlar.Items.Add("--------------------------");

            // 8) Bulduğumuz her adım için hem metinsel hem de görsel sunum hazırlıyoruz.
            for (int i = 0; i < yol.Count; i++)
            {
                // 8a) ListBox'a adım numarasını ve durumu yazdırıyoruz.
                lstCozumAdimlar.Items.Add("Adım " + i + ":");
                lstCozumAdimlar.Items.Add(DurumuYazdir(yol[i].Durum));
                // F = G + H değerlerini de görüntülüyoruz.
                lstCozumAdimlar.Items.Add("F = G + H = " + yol[i].G + " + " + yol[i].H + " = " + yol[i].F);
                lstCozumAdimlar.Items.Add("--------------------------");

                // 8b) Her adım için küçük bir panel oluşturup, adımın 3x3 halini görsel olarak göstereceğiz.
                Panel adimPanel = new Panel();
                adimPanel.Name = "adimPanel_" + i;                // Panelin ismini 'adimPanel_i' olarak veriyoruz.
                adimPanel.BorderStyle = BorderStyle.FixedSingle;   // Panelin kenarlık stilini ayarlıyoruz.
                adimPanel.Size = new Size(150, 190);               // Panelin boyutu.

                // 8c) Panelleri ekranda 3 sütun halinde yerleştirmek için konumu hesaplıyoruz.
                //     (i % 3) * 190 => soldan sağa doğru, (i / 3) * 210 => yukarıdan aşağıya doğru.
                adimPanel.Location = new Point(20 + (i % 3) * 190, 270 + (i / 3) * 210);

                // 8d) Adım numarasını gösteren bir etiket ekliyoruz.
                Label lblAdim = new Label();
                lblAdim.Text = "Adım " + i;
                lblAdim.Location = new Point(5, 5);
                lblAdim.AutoSize = true;
                lblAdim.Font = new Font(lblAdim.Font, FontStyle.Bold);
                adimPanel.Controls.Add(lblAdim);

                // 8e) 3x3 matris üzerinde her hücre için bir Button oluşturup, durumun değerini gösteriyoruz.
                //     Bu butonlar sadece görüntü amaçlı, tıklanamaz (Enabled=false).
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        Button btn = new Button();
                        btn.Size = new Size(40, 40);
                        btn.Location = new Point(5 + c * 45, 30 + r * 45);
                        btn.Enabled = false;

                        int deger = yol[i].Durum[r, c];
                        // Eğer değer 0 ise buton metni boş kalır, değilse sayıyı yazar.
                        btn.Text = deger == 0 ? "" : deger.ToString();

                        // Bu kod, o anki adımın 3x3 durumunda bulunan bir taşın (deger) hedef durumdaki doğru konumunda olup olmadığını kontrol eder.
                        // 'hr' ve 'hc', hedefDurum matrisinin satır ve sütun indeksleridir (0'dan 2'ye kadar döner).
                        // Döngü içinde, hedefDurum[hr, hc] == deger ile, hedef durumdaki ilgili hücrenin de aynı değere sahip olup olmadığına bakar.
                        // Aynı değere sahip olmak yeterli değildir; aynı konumda (hr == r ve hc == c) olması gerekir.
                        // Eğer taş doğru yerdeyse 'hedefteDogruYerde = true' yapılır ve döngüden çıkılır.
                        // Ardından 'hedefteDogruYerde' true ise butonun arka plan rengi yeşil (Color.LightGreen) yapılır, 
                        // yani bu taşın hedef konumda olduğu görsel olarak vurgulanır.
                        // 'hr' ve 'hc', hedefDurum matrisinde satır (row) ve sütun (column) indekslerini temsil eden değişkenlerdir.
                        // İsimlendirmede 'hr' (hedef row) ve 'hc' (hedef column) olarak düşünülebilir; yani hedef matrisindeki satır ve sütunu gösterir.
                        // Bu döngüler, hedefDurum içindeki 3x3 hücreleri tek tek kontrol ederek, taşın (deger) tam olarak hangi konumda olduğunu belirlemeye yarar.

                        if (deger != 0)
                        {
                            bool hedefteDogruYerde = false;
                            for (int hr = 0; hr < 3; hr++)
                            {
                                for (int hc = 0; hc < 3; hc++)
                                {
                                    if (hedefDurum[hr, hc] == deger && hr == r && hc == c)
                                    {
                                        hedefteDogruYerde = true;
                                        break;
                                    }
                                }
                            }
                            if (hedefteDogruYerde)
                            {
                                btn.BackColor = Color.LightGreen;
                            }
                        }


                        // Oluşturulan butonu adım paneline ekliyoruz.
                        adimPanel.Controls.Add(btn);
                    }
                }

                // 8g) Bu adımın F, G, H değerlerini de panelin alt kısmında göstermek için etiket oluşturuyoruz.
                Label lblFGH = new Label();
                lblFGH.Text = "F = " + yol[i].F + " (G=" + yol[i].G + ", H=" + yol[i].H + ")";
                lblFGH.Location = new Point(5, 165);
                lblFGH.AutoSize = true;
                adimPanel.Controls.Add(lblFGH);

                // 8h) Paneli formun kontrol koleksiyonuna ekliyoruz,
                //     böylece her adımın küçük bir görsel temsili ekranda gösterilecek.
                this.Controls.Add(adimPanel);
            }

            // 'yeniYukseklik' değişkeni, formun dikey boyutunu belirler.
            //  - Math.Max(700, ...) ifadesi, formun yüksekliğinin en az 700 piksel olmasını sağlar.
            //  - (yol.Count - 1) / 3 + 1, çözüm adımlarını 3'lü sütunlar halinde yerleştirirken kaç "satır" gerektiğini hesaplar.
            //  - Her satır için 210 piksel (panel yüksekliği + aralıklar) eklenir, 280 piksel de üst kısım boşluğu içindir.
            // 'this.Size' ile formun genişlik ve yükseklik değerleri atanır:
            //  - Genişlik için Math.Max(1000, 20 + Math.Min(yol.Count, 3) * 190 + 150) ifadesi,
            //    en az 1000 piksel genişlik veya en fazla 3 panel sığacak kadar genişlik sağlar.
            // Sonuç olarak, adım panellerinin sayısına göre formun boyutu dinamik olarak ayarlanır.
            int yeniYukseklik = Math.Max(700, 280 + ((yol.Count - 1) / 3 + 1) * 210);
            this.Size = new Size(
                Math.Max(1000, 20 + Math.Min(yol.Count, 3) * 190 + 150),
                yeniYukseklik
            );

        }


        // *** DURUMU METİN OLARAK YAZDIRMA METOTU
        // 3x3 matris şeklindeki durumu satır satır string olarak oluşturur.
        private string DurumuYazdir(int[,] durum)
        {
            // 'sonuc' adlı birikimli string değişkeni, matrisin metin halini oluşturmak için kullanılır.
            string sonuc = "";

            // 3x3 matrisin satırları için (i) ve sütunları için (j) iki döngü kullanılır.
            // Matrisin satır sayısı 3 olduğu için, döngü 0'dan başlayıp 2'ye (i < 3) kadar çalışır.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Eğer hücre değeri 0 ise "  " (iki boşluk), değilse sayıyı ve bir boşluk ekler.
                    // 0 değeri boş hücreyi temsil eder; bu metinde sadece boşluk olarak gösterilir.
                    sonuc += durum[i, j] == 0 ? "  " : durum[i, j] + " ";
                }
                // Her satırın sonunda yeni bir satıra geçilir (Environment.NewLine).
                sonuc += Environment.NewLine;
                // 'Environment.NewLine', işletim sistemine özgü satır sonu (newline) karakterlerini temsil eder.
                // Örneğin Windows'ta "\r\n" (carriage return + line feed), Linux ve macOS'ta "\n" olarak kullanılır.

            }

            // Biriken metinsel sonucu geriye döndürür.
            return sonuc;
        }


        // *** A* ALGORİTMASINDA KULLANILAN DÜĞÜM SINIFI
        private class Dugum
        {
            // 'Durum': Bu düğümdeki 3x3 matrisin mevcut halini (bulmacadaki taşların konumlarını) tutar.
            public int[,] Durum { get; set; }

            // 'G': Başlangıç durumundan bu düğüme gelene kadar yapılan hamlelerin toplam maliyetini temsil eder.
            public int G { get; set; }

            // 'H': Hedefe kalan tahmini maliyettir. Burada Manhattan mesafesi gibi bir sezgisel (heuristic) kullanılır.
            public int H { get; set; }

            // 'F': Toplam maliyettir (F = G + H). A* algoritmasında, en düşük F değerine sahip düğüm öncelikli işlenir.
            public int F { get; set; }

            // 'Parent': Bu düğümün, bir önceki (ebeveyn) düğümüdür.
            // Çözüm yolu bulununca, Parent üzerinden geriye gidilerek adım adım başlangıca ulaşılabilir. , parent ebeveyn
            public Dugum Parent { get; set; }

            // Yapıcı (constructor) metot: Düğüm oluşturulurken Durum, G, H, Parent değerleri verilir.
            // F değeri ise G + H olarak hesaplanır.
            public Dugum(int[,] durum, int g, int h, Dugum parent)
            {
                Durum = durum;
                G = g;
                H = h;
                F = g + h;
                Parent = parent;
                // F: Toplam maliyet (F = G + H). 
                // G: Başlangıç noktası (A) ile mevcut konum arasındaki gerçek maliyet (şu ana kadar kat edilen yol).
                // H: Mevcut konumdan hedef noktaya (B) olan tahmini maliyet.
                //    Bu tahmin (heuristic), gerçekte ne kadar yol kalabileceğini yaklaşık olarak hesaplar.
                //    Örneğin, Manhattan mesafesi veya öklid mesafesi bu amaçla kullanılabilir.
                // Heuristic (tahmini/sezgisel) değer, hedefe uzaklığı tahmin ederek arama sürecini hızlandırmaya yardımcı olur.

            }
        }
        // *** A* ALGORİTMA METOTU
        // A* algoritmasının genel formülü: f(x) = a*g(x) + (1-a)*h(x) şeklindedir.
        // A* algoritmasının temel felsefesi, hem şu ana kadar kat edilen gerçek maliyeti (g(x)) 
        // hem de hedefe kalan tahmini maliyeti (h(x)) dikkate alarak en verimli yolu bulmaktır.
        // f(x) = a*g(x) + (1-a)*h(x) formülü, bu iki değerin önemini ayarlamak için 'a' katsayısını kullanır.
        // Örneğin a=1 iken, f(x) = g(x) + h(x) olarak sadeleşir ve A* algoritmasının klasik hali elde edilir.
        // Burada g(x), başlangıçtan mevcut duruma kadar olan maliyet,
        // h(x) ise mevcut durumdan hedefe kalan tahmini maliyettir.
        // 'a' katsayısı 1'e eşit kabul edilirse, f(x) = g(x) + h(x) formülüne dönüşür
        // ve kodumuzda bu sadeleştirilmiş versiyon kullanılmaktadır.
        // Başlangıç durumundan hedef duruma ulaşmak için A* algoritması uygulanır.
        private List<Dugum> AStar(int[,] baslangic, int[,] hedef)
        {
            // Henüz işlenmemiş düğümleri saklayan açık liste
            List<Dugum> acikListe = new List<Dugum>();
            // İşlemi tamamlanmış düğümleri saklayan kapalı liste
            List<Dugum> kapaliListe = new List<Dugum>();

            // 'baslangicH' değişkeni, başlangıç durumunun hedefe olan tahmini uzaklığını (H) hesaplar (Manhattan mesafesi vb.).
            int baslangicH = HesaplaH(baslangic, hedef);

            // 'baslangicDugum' adlı yeni bir düğüm (node) oluşturulur.
            // - İlk parametre 'baslangic' durumu (3x3 matris)
            // - G = 0, yani başlangıçtan buraya kadarki maliyet 0'dır (henüz yol kat edilmedi)
            // - H = 'baslangicH' (hedefe kalan tahmini maliyet)
            // - Parent = null (henüz önceki düğüm yok, çünkü burası başlangıç)
            Dugum baslangicDugum = new Dugum(baslangic, 0, baslangicH, null);

            // 'acikListe' (open list), henüz işlenmemiş düğümleri saklayan bir koleksiyondur.
            // Başlangıç düğümünü bu listeye ekleyerek A* algoritmasını başlatmış oluruz.
            acikListe.Add(baslangicDugum);


            // 'maxIterasyon' değişkeni, A* döngüsünün sonsuza kadar çalışmasını önlemek için belirlenen üst sınırdır.
            // 10.000 rastgele seçilmiş, yeterince büyük bir sayı olup, olağan koşullarda algoritmanın
            // sonsuz döngüye girmesini engellemeye yöneliktir.
            int maxIterasyon = 10000;
            int iterasyon = 0;


            // Bu while döngüsü, A* algoritmasının kalbidir; açık listede düğüm kaldığı sürece veya belirlenen maksimum 
            // iterasyon (maxIterasyon) aşılmadığı sürece dönmeye devam eder.
            // Bu fonksiyonun çalışma sistemi özetle şöyledir:
            // 1) Açık liste (acikListe), işlemeyi bekleyen düğümleri tutar. Kapalı liste (kapaliListe), işlenmiş düğümleri saklar.
            // 2) Döngü, açık listede düğüm kaldığı veya maxIterasyon aşılmadığı sürece sürer.
            // 3) Açık listeden F değeri en düşük (ve eşitlikte H değeri en düşük) düğüm (simdiki) seçilir.
            // 4) Eğer simdiki düğümün durumu hedef durumla aynıysa, Parent bağlantılarıyla çözüm yolu oluşturulur ve döndürülür.
            // 'simdiki' düğümün durumu, 'simdiki.Durum' ifadesiyle erişilen 3x3 matristir.
            // Hedef durum ise 'hedef' değişkeninde (int[,] tipinde) tutulur.
            // 'DurumlarEsitMi(simdiki.Durum, hedef)' karşılaştırması, bu iki matrisin
            // içerik bakımından birebir aynı olup olmadığını kontrol eder.
            // 5) Aksi takdirde, simdiki düğüm açık listeden çıkarılıp kapalı listeye eklenir (işlenmiş sayılır).
            // 6) Boş hücrenin (0) konumu (bosX, bosY) bulunur. Yukarı, aşağı, sol, sağ hareketlerle yeni durumlar oluşturulur.
            // 7) Yeni durumlar kapalı listede yoksa açık listeye eklenir ya da daha kısa yol keşfedildiyse (yeniG < mevcut.G) güncellenir.
            // 8) Döngü, hedef bulunana veya açık liste tükenene kadar devam eder.
            // Böylece A* algoritması, her adımda hedef duruma en kısa yoldan ulaşmayı amaçlar.

            while (acikListe.Count > 0 && iterasyon < maxIterasyon)
            {
                iterasyon++;  // 1) Her döngüde iterasyon sayısını bir artırarak, sonsuz döngüye girilmesini engellemeye çalışıyoruz.

                // 2) Açık listede (acikListe) bulunan düğümler arasından, F değeri en düşük olan düğüm seçilir.
                //    Eşitlik varsa, ThenBy(n => n.H) ile H değeri daha düşük olanı tercih edilir.
                // 'acikListe', henüz işlenmemiş düğümlerin tutulduğu bir listedir.
                // 'OrderBy(n => n.F)' ifadesi, listedeki düğümleri 'F' değerine göre küçükten büyüğe sıralar.
                // 'ThenBy(n => n.H)', F eşitse H değerine göre sıralamayı sürdürür.
                // 'First()', sıralanmış listenin ilk (en küçük) elemanını alır.
                // Burada 'n' => 'n' her bir Dugum (düğüm) nesnesini temsil eder,
                // 'F' özelliği, A* algoritmasında toplam maliyeti (G + H) temsil eder.
                // 'H' özelliği, hedefe kalan tahmini mesafeyi (heuristic) ifade eder.
                // Yani bu satır, açık listede F değeri en düşük olan düğümü (ve eşitlik varsa H değeri en düşük olanı)
                // 'simdiki' değişkenine atar.
                Dugum simdiki = acikListe.OrderBy(n => n.F).ThenBy(n => n.H).First();


                // 3) Eğer 'simdiki' düğümün durumu, hedef durumla aynıysa çözümü bulduk demektir.
                if (DurumlarEsitMi(simdiki.Durum, hedef))
                {
                    // 3a) Çözüm yolunu tutmak için bir liste oluşturuyoruz.
                    List<Dugum> yol = new List<Dugum>();
                    Dugum simdikiYol = simdiki;

                    // 3b) Parent üzerinden geriye giderek (son düğümden başlangıca), 
                    //     çözümün geçtiği tüm düğümleri 'yol' listesine ekliyoruz.
                    while (simdikiYol != null)
                    {
                        yol.Add(simdikiYol);
                        simdikiYol = simdikiYol.Parent;
                    }

                    // 3c) 'yol' şu an ters sırada (hedeften başlayıp başlangıca kadar). Reverse() ile düz sıraya çeviririz. , reverse tersi demek
                    yol.Reverse();

                    // 3d) Bulunan çözüm yolunu döndürerek metodu sonlandırırız.
                    return yol;
                }

                // 4) Eğer hedefe ulaşmadıysak, 'simdiki' düğümü artık işlenmiş sayarız:
                //    Açık listeden çıkarıp kapalı listeye ekleriz. Böylece tekrar seçilmez.
                acikListe.Remove(simdiki);
                kapaliListe.Add(simdiki);

                // 5) 'simdiki' durumunda boş hücre (0) neredeyse (bosX, bosY) olarak bulalım.
                int bosX = 0, bosY = 0;
                BosHucreyiBul(simdiki.Durum, ref bosX, ref bosY);

                // 6) Boş hücrenin etrafındaki dört olası komşu konum (yukarı, aşağı, sol, sağ) tanımlanır.
                //    dx ve dy dizileri bu yönde hareket etmeyi sağlar.
                int[] dx = { -1, 1, 0, 0 };
                int[] dy = { 0, 0, -1, 1 };

                // 7) Bu dört komşunun her biri için yeni bir durum oluşturup değerlendireceğiz.
                for (int i = 0; i < 4; i++)
                {
                    // 7a) Boş hücrenin yeni konumunu hesapla (yukarı, aşağı, sol, sağ).
                    int yeniX = bosX + dx[i];
                    int yeniY = bosY + dy[i];

                    // 7b) Eğer (yeniX, yeniY) 3x3 matrisin sınırları içindeyse devam et.
                    if (yeniX >= 0 && yeniX < 3 && yeniY >= 0 && yeniY < 3)
                    {
                        // 7c) Mevcut durumun (3x3 matrisin) kopyasını oluşturup,
                        //     boş hücreyle komşu hücreyi yer değiştiririz.
                        int[,] yeniDurum = DurumKopyala(simdiki.Durum);
                        yeniDurum[bosX, bosY] = yeniDurum[yeniX, yeniY];
                        yeniDurum[yeniX, yeniY] = 0;

                        // 7d) Eğer bu yeni durum, kapalı listede (işlenmiş düğümler) yoksa incelemeye değer.
                        if (!ListedeVarMi(kapaliListe, yeniDurum))
                        {
                            // 7e) 'yeniG', simdiki düğümün G değerine +1 eklenerek hesaplanır 
                            //     (her taş kaydırma hamlesi 1 maliyet).
                            int yeniG = simdiki.G + 1;

                            // 7f) 'yeniH', bu yeni durumun hedef durumdan ne kadar uzakta olduğunu 
                            //     tahmin eden sezgisel (heuristic) fonksiyonla hesaplanır (HesaplaH).
                            int yeniH = HesaplaH(yeniDurum, hedef);

                            // 7g) Açık listede, bu 'yeniDurum' ile aynı olan bir düğüm var mı, kontrol edilir.
                            Dugum mevcut = ListedeBul(acikListe, yeniDurum);

                            // 7h) Eğer henüz yoksa, yeni bir düğüm oluşturup açık listeye ekleriz.
                            if (mevcut == null)
                            {
                                Dugum yeniDugum = new Dugum(yeniDurum, yeniG, yeniH, simdiki);
                                acikListe.Add(yeniDugum);
                            }
                            // 7i) Eğer açık listede aynı durum zaten varsa ve yeniG < mevcut.G ise,
                            //     bu daha kısa bir yol demektir; mevcut düğümü güncelleriz.
                            else if (yeniG < mevcut.G)
                            {
                                mevcut.G = yeniG;
                                mevcut.F = yeniG + mevcut.H;  // F değeri de güncellenmeli (G+H).
                                mevcut.Parent = simdiki;      // Ebeveyn düğümünü güncel (simdiki) yapıyoruz.
                            }
                        }
                    }
                }
            }


            // Açık liste boşalırsa veya maksimum iterasyon aşıldıysa çözüm bulunamamıştır
            return null;
        }

        // *** SEZGİSEL DEĞER HESAPLAMA (MANHATTAN MESAFESİ)
        // Bu metot, mevcut durumdaki her taşın hedef durumdaki konumundan ne kadar uzakta olduğunu
        // "Manhattan mesafesi" yöntemiyle hesaplar (satır farkı + sütun farkı)
        private int HesaplaH(int[,] durum, int[,] hedef)
        {
            int toplam = 0;  // Tüm taşların hedefe uzaklıklarının toplamı

            // 1) 3x3 matrisin her hücresini (i, j) dolaşır.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int deger = durum[i, j];  // Mevcut hücredeki taş (0 hariç)

                    // 2) Eğer deger 0 değilse (yani boşluk değilse), hedefteki konumunu bulur.
                    if (deger != 0)
                    {
                        int hedefX = 0, hedefY = 0;

                        // 3) Hedef matris içinde, bu 'deger' (taş) hangi (x, y) konumundaysa onu bulur.
                        for (int x = 0; x < 3; x++)
                        {
                            for (int y = 0; y < 3; y++)
                            {
                                if (hedef[x, y] == deger)
                                {
                                    hedefX = x;
                                    hedefY = y;
                                    break;  // Aradığımız taş bulununca iç döngüden çık.
                                }
                            }
                        }

                        // 4) Manhattan mesafesi: satır farkı (|i - hedefX|) + sütun farkı (|j - hedefY|).
                        //    'Math.Abs' mutlak değer hesaplar.
                        toplam += Math.Abs(i - hedefX) + Math.Abs(j - hedefY);
                    }
                }
            }

            // 5) Tüm taşların hedefe olan uzaklıklarının toplamını döndürür.
            return toplam;
        }

        // *** İKİ DURUMUN EŞİTLİĞİNİ KONTROL ETME
        // 'DurumlarEsitMi' metodu, iki farklı 3x3 matrisin (durum1 ve durum2) aynı içerikte olup olmadığını inceler.
        // 1) 3x3'lük her hücreyi (i, j) dolaşır.
        // 2) Bir hücredeki değerler farklıysa (durum1[i, j] != durum2[i, j]), 'false' döndürür.
        // 3) Tüm hücreler aynı ise, döngü bittiğinde 'true' döndürerek iki matrisin aynı olduğunu belirtir.
        private bool DurumlarEsitMi(int[,] durum1, int[,] durum2)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (durum1[i, j] != durum2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // *** DURUM KOPYALAMA METOTU
        // 'DurumKopyala' metodu, verilen 3x3 matrisin (durum) içeriğini yeni bir 3x3 diziye kopyalar.
        // 1) yeniDurum adında 3x3 boyutlu boş bir dizi oluşturulur.
        // 2) Tüm hücreler (i, j) dolaşılarak, durum[i, j] değeri yeniDurum[i, j]'ye atanır.
        // 3) Kopyalama tamamlandığında yeniDurum döndürülür.
        // Bu sayede orijinal matrisle tamamen aynı verilere sahip ayrı bir matris elde edilir,
        // böylece orijinali değiştirmeden üzerinde işlem yapılabilir.
        // Bu metot hem başlangıç durumu hem de hedef durum (ya da başka herhangi bir 3x3 durum) için kullanılabilir.
        // 'DurumKopyala', parametre olarak aldığı 3x3 matrisin (örneğin başlangıç veya hedef durumu)
        // içeriğini değiştirmek istemediğimizde veya aynı verileri başka bir yerde kullanmak istediğimizde işe yarar.
        // Böylece orijinal matris bozulmadan, onun kopyası üzerinde işlemler yapılabilir.
        // Uygulamada, A* algoritması içinde yeni durumlar (yeniDurum) oluştururken 'DurumKopyala' fonksiyonu kullanılır.
        // Örneğin, boş hücre ile komşu hücrenin yer değiştirmesi gibi işlemleri yaparken
        // orijinal 'simdiki.Durum' matrisini bozmadan yeni bir matris üzerinde değişiklik yapılması gerekir.
        // Bu yüzden fonksiyon gereklidir; aksi takdirde orijinal matris değişmiş olur ve
        // arama algoritmasının mantığı bozulur (eski durum geri alınamaz).
        // A* algoritması, bir arama (search) yaklaşımı kullanır. Farklı hamleler (boş hücrenin yer değiştirmesi) denenir,
        // her yeni durum (yeniDurum) açık listeye eklenir. Eğer orijinal durumu değiştirseydik, başka bir hamle için
        // geri dönüp farklı bir yolu deneme şansımız olmazdı ya da önceki durumları doğru saklayamazdık.
        // Bu yüzden, her denemede durum kopyalanarak yeni bir matris elde edilir; böylece arama sürecinde
        // farklı dallar (farklı hamleler) güvenle keşfedilebilir.
        private int[,] DurumKopyala(int[,] durum)
        {
            int[,] yeniDurum = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    yeniDurum[i, j] = durum[i, j];
                }
            }
            return yeniDurum;
        }

        // *** BOŞ HÜCREYİ BULMA METOTU
        // 'BosHucreyiBul' metodu, 3x3 matris içindeki '0' (boş hücre) değerinin satır (x) ve sütun (y) konumunu bulur.
        // 'ref int x' ve 'ref int y' parametreleri sayesinde, bulduğu konum değerlerini bu değişkenlere geri aktarır.
        private void BosHucreyiBul(int[,] durum, ref int x, ref int y)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (durum[i, j] == 0)
                    {
                        // Boş hücre bulununca, konum x ve y'ye atanır ve metot sonlandırılır (return).
                        x = i;
                        y = j;
                        return;
                    }
                }
            }
        }

        // *** LİSTEDE VERİLEN DURUMUN VARLIĞINI KONTROL ETME
        // 'ListedeVarMi' metodu, 'liste' içindeki herhangi bir 'Dugum' nesnesinin 
        // 'Durum' özelliğinin 'durum' adlı 3x3 matrisle aynı olup olmadığını kontrol eder.
        // 'durum', 3x3 boyutunda bir matristir (int[,] tipinde), her hücresinde 0-8 arası değerler tutulur.
        // 'dugum.Durum' da aynı şekilde 3x3'lük bir matristir. 'DurumlarEsitMi' metodu,
        // her iki matrisin (durum ve dugum.Durum) tüm koordinatlarını (i,j) tek tek karşılaştırır.
        // Yani 0,0'dan başlayıp 2,2'ye kadar bütün hücrelerdeki değerler aynı mı diye bakar.
        // Eğer tüm hücreler birebir aynıysa iki durum eşittir, aksi halde farklıdır.

        private bool ListedeVarMi(List<Dugum> liste, int[,] durum)
        {
            // 'foreach' döngüsüyle listedeki her bir düğümün durumunu karşılaştırır.
            foreach (Dugum dugum in liste)
            {
                // 'DurumlarEsitMi' metodu, iki matrisin birebir aynı olup olmadığını kontrol eder.
                if (DurumlarEsitMi(dugum.Durum, durum))
                {
                    // Aynı durumu bulursa 'true' döndürür.
                    return true;
                }
            }
            // Listedeki hiçbiri eşleşmezse 'false' döndürür.
            return false;
        }

        // *** LİSTEDE VERİLEN DURUMU BULMA
        // 'ListedeBul' metodu, 'liste' içinde 'durum' adlı 3x3 matrisle aynı 'Durum' özelliğine sahip düğümü arar.
        // Eğer bulursa ilgili 'Dugum' nesnesini döndürür, bulamazsa 'null' döndürür.
        private Dugum ListedeBul(List<Dugum> liste, int[,] durum)
        {
            foreach (Dugum dugum in liste)
            {
                if (DurumlarEsitMi(dugum.Durum, durum))
                {
                    // Aynı durum bulunursa, o düğüm geri döndürülür.
                    return dugum;
                }
            }
            // Hiç eşleşme yoksa null döner.
            return null;
        }

    }
}
// 'durum', 3x3 boyutunda bir matristir (int[,] tipinde), her hücresinde 0-8 arası değerler tutulur.
// 'dugum.Durum' da aynı şekilde 3x3'lük bir matristir. 'DurumlarEsitMi' metodu,
// her iki matrisin (durum ve dugum.Durum) tüm koordinatlarını (i,j) tek tek karşılaştırır.
// Yani 0,0'dan başlayıp 2,2'ye kadar bütün hücrelerdeki değerler aynı mı diye bakar.
// Eğer tüm hücreler birebir aynıysa iki durum eşittir, aksi halde farklıdır.

// A* algoritmasında kullanılan F = G + H formülü “yapay zeka”ya özgü bir denklem gibi görünmese de,
// aslında “heuristic (sezgisel)” kavramıyla yapay zeka sahasına girer. 
// Çünkü H değeri, hedefe kalan mesafeyi tahmin eden bir fonksiyondur (Manhattan mesafesi vb.).
// Bu tahmin, arama sürecini “akıllı” hale getirir ve algoritmanın gereksiz yolları elemesine yardımcı olur.
// Dolayısıyla “formül” olarak görünen bu yaklaşım, arka planda bir yapay zeka tekniği olan
// “bilgili (informed) arama” yönteminin temelini oluşturur.

// Bu projede Manhattan mesafesi, hazır bir kütüphaneden değil, elle yazılan döngülerle hesaplanıyor.
// Örneğin 'HesaplaH' metodu, her bir taşın (0 hariç) hedefteki konumunu bularak
// satır farkı + sütun farkını topluyor. Böylece "hazır fonksiyon" kullanmadan
// A* algoritmasının gerektirdiği sezgisel (heuristic) değeri manuel olarak elde ediyoruz.
// Aynı şekilde, A* algoritması da tamamen el ile yazılmış döngüler ve listeler üzerinden çalışıyor.
// Dolayısıyla “kütüphane fonksiyonları” yerine, temel kodlama teknikleriyle arama mantığı inşa edilmiş oluyor.

//
// !!!*** KODUN GENEL İŞLEYİŞİ VE A* ALGORİTMASININ MANTIĞI **!!!!!
//
// Bu kod, 8-Taş Bulmacası'nı (3x3'lük bir bulmaca) A* algoritması kullanarak çözmek için tasarlanmıştır.
// Aşağıda kodun genel çalışma mantığını ve hangi fonksiyonun ne işe yaradığını özetleyen detaylı bir açıklama yer alıyor.
// Kod incelenirken, hem Windows Forms elemanlarının (paneller, butonlar, etiketler, ListBox vb.) nasıl oluşturulduğuna
// hem de A* algoritmasının adım adım nasıl uygulandığına odaklanmak gerekir.
//
// 1) Form1 Sınıfı ve Constructor (public Form1())
//    - Bu sınıf, Windows Forms uygulamasının ana penceresini (Form) temsil eder.
//    - Constructor içinde, formun başlığı (Text), boyutu (Size) ve bazı ayarlar (AutoScroll, AutoScaleMode) yapılır.
//    - Ardından 'FormOlustur()' metodu çağrılarak, ekranda görünecek paneller, butonlar, etiketler ve ListBox gibi
//      kontroller oluşturulur.
//
// 2) FormOlustur() Metodu
//    - Başlangıç paneli (baslangicPanel) ve hedef paneli (hedefPanel) gibi iki farklı Panel kontrolü oluşturulur.
//    - "Başlangıç Durumu" ve "Hedef Durumu" yazan Label'lar eklenir.
//    - "A* ile Çöz" adında bir buton oluşturularak, tıklandığında 'BtnCoz_Click' metodunu çağırması sağlanır.
//    - Başlangıç paneli üzerine, for döngüleriyle 3x3 adet buton oluşturulur (baslangicDurum). Bu butonlar
//      8-Taş Bulmacası'nın başlangıç konumunu temsil eden sayıları (veya 0 için boşluk) içerir.
//      - Her butonun Tag özelliğine (i, j) konumu kaydedilir, tıklandığında 'BaslangicButon_Click' metodu çalışır.
//      - Ayrıca 'baslangicDurum' adlı 3x3 diziye bu sayılar atanır, böylece kodda da saklanır.
//    - Hedef paneli üzerine de yine 3x3 adet buton oluşturulur (hedefDurum). Bu butonlar bulmacanın hedef konumunu
//      temsil eden sayıları içerir ve tıklanınca 'HedefButon_Click' metodu çağrılır.
//
// 3) BaslangicButon_Click(object sender, EventArgs e)
//    - Başlangıç panelindeki bir butona tıklanırsa çalışır. Kullanıcı, o butonda yazan değeri değiştirebilir.
//    - Küçük bir form açılır (inputForm), kullanıcıdan 0-8 arasında yeni bir değer girmesi istenir.
//    - Girilen değer valid (geçerli) ise ve başka bir hücrede yoksa, 'baslangicDurum' dizisi güncellenir,
//      butonun metni de yeni değere göre ayarlanır.
//
// 4) HedefButon_Click(object sender, EventArgs e)
//    - Hedef panelindeki bir butona tıklanırsa çalışır. Aynı mantıkla, kullanıcı hedef durumun ilgili hücresini
//      değiştirebilir. Bu sayede hedef durumu da dinamik olarak düzenleme imkânı sunar.
//
// 5) BtnCoz_Click(object sender, EventArgs e)
//    - "A* ile Çöz" butonuna basıldığında, esas A* algoritması devreye girer.
//    - Daha önceki çözümlerden kalan ListBox içeriği ve adım panelleri temizlenir.
//    - 'yol = AStar(baslangicDurum, hedefDurum);' satırıyla, A* algoritması çağrılır ve
//      başlangıç durumundan hedef duruma giden çözüm adımları elde edilir.
//    - Eğer yol boşsa, çözüm yoktur. Aksi takdirde, her adım ListBox’a yazılır ve ayrıca formda küçük paneller
//      (adimPanel) oluşturarak görsel olarak da gösterilir (3x3 butonlar şeklinde).
//    - Formun boyutu, adım sayısına göre dinamik olarak büyütülür.
//
// 6) AStar(int[,] baslangic, int[,] hedef) Metodu
//    - Kodun yapay zeka  kısmıdır. A* algoritmasının ana mantığı buradadır:
//      a) 'acikListe' (open list): Henüz işlenmemiş düğümler (durumlar).
//      b) 'kapaliListe' (closed list): İşlenmiş düğümler.
//
//      - Başlangıç durumu (baslangic) için G=0, H=HesaplaH(...) ile hesaplanır ve F=G+H şeklinde bulunur.
//        Bu başlangıç düğümü açık listeye eklenir.
//      - while döngüsünde, açık liste boş olmadığı sürece devam edilir (veya iterasyon < maxIterasyon).
//      - Açık listede F değeri en düşük (eşitse H değeri en düşük) düğüm seçilir (OrderBy/ThenBy). Bu 'simdiki' düğümdür.
//      - Eğer 'simdiki.Durum' hedef durumla aynı ise, Parent bağlantıları takip edilerek (geriye doğru) çözüm yolu
//        oluşturulur ve döndürülür.
//      - Değilse, 'simdiki' düğümü kapalı listeye aktarılır (artık işlenmiş sayılır).
//      - Boş hücrenin konumu bulunur (BosHucreyiBul). Boş hücreyle komşu 4 konum (yukarı, aşağı, sol, sağ) denenerek
//        yeni durumlar (yeniDurum) oluşturulur. Bu yeni durumlar kapalı listede yoksa, açık listeye eklenir
//        (veya daha iyi bir yol bulunduysa güncellenir).
//      - Döngü bu şekilde sürer, ta ki hedef durum bulunana veya açık liste tükenene kadar.
//
// 7) HesaplaH(int[,] durum, int[,] hedef)
//    - A* algoritmasındaki H (heuristic) değerini hesaplar. Burada Manhattan mesafesi kullanılmıştır.
//    - Her taşın (0 hariç) hedef konumuna satır farkı + sütun farkı şeklinde bakarak toplam uzaklığı döndürür.
//
// 8) DurumlarEsitMi(int[,] durum1, int[,] durum2)
//    - İki 3x3 matrisin her hücresini karşılaştırarak birebir aynı olup olmadıklarını kontrol eder.
//    - A* içinde "Bu durum zaten kapalı listede mi?" veya "Hedef durumla aynı mı?" gibi yerlerde kullanılır.
//
// 9) DurumKopyala(int[,] durum)
//    - 3x3 matrisin yeni bir kopyasını oluşturur. Böylece orijinal durumu bozmadan yeni durumlar üzerinde işlem yapabiliriz.
//    - A* içinde boş hücreyle komşu hücreyi yer değiştirme denemesi yaparken, bu kopya fonksiyonuna ihtiyaç duyulur.
//
// 10) BosHucreyiBul(int[,] durum, ref int x, ref int y)
//     - Verilen 3x3 matris içinde değeri 0 olan hücrenin konumunu (x,y) bulur.
//     - A* içinde, hangi taşın kaydırılacağını belirlemek için gereklidir (0 hücre, yani boşluk).
//
// 11) ListedeVarMi(List<Dugum> liste, int[,] durum) ve ListedeBul(List<Dugum> liste, int[,] durum)
//     - 'ListedeVarMi', kapalı liste gibi bir koleksiyonda, aynı durumun (3x3 matrisin) var olup olmadığını sorgular.
//     - 'ListedeBul', açık listede o durumun hangi 'Dugum' nesnesine karşılık geldiğini bulur. Bulursa döndürür,
//       bulamazsa null döndürür.
//     - Bu yöntemlerle, yeni üretilen durumların daha önce işlenip işlenmediğini veya açık listede olup olmadığını
//       kontrol ederiz.
//
// *** SONUÇ VE GENEL BAKIŞ ***
//
// - Bu kod, Windows Forms ile kullanıcı arayüzü oluşturarak 8-Taş Bulmacası’nı çözmek için A* algoritmasını uygular.
// - Kullanıcı başlangıç ve hedef durumunu (butonlara tıklayarak) değiştirebilir. Ardından "A* ile Çöz" butonuna
//   tıklandığında, kod AStar fonksiyonunu çalıştırır ve bulduğu çözüm adımlarını hem ListBox'ta metin olarak
//   hem de küçük panellerde 3x3 buton düzeniyle görsel olarak gösterir.
// - A* algoritması, yapay zekanın “bilgili (informed) arama” yöntemlerinden biridir. 'H' (heuristic) değeri sayesinde,
//   hedefe ulaşma sürecini hızlandırır ve gereksiz dalları erken eler. Böylece optimal veya optimal yakın bir
//   çözüm elde edilebilir.
// - Kodda kullanılan sezgisel fonksiyon (Manhattan mesafesi), 8-Taş Bulmacası gibi kaydırma tabanlı problemlerde
//   yaygın ve uygun bir tahmin yöntemi sağlar. 
// - Bu uygulama, yapay zekanın temel arama algoritmalarından birini (A*) pratik bir örnek üzerinde gösterir,
//   böylece hem algoritma mantığını hem de Windows Forms üzerinden kullanıcı etkileşimini öğrenme fırsatı sunar.
