# CartMVCApp — ASP.NET Core MVC 5.0 Örnek E-Ticaret Sitesi

Giriş/kayıt sistemi, ürün listesi, sepete ürün ekleme ve siparişi onaylama
özelliklerini içeren örnek bir ASP.NET Core MVC (.NET 5.0) projesidir.

## Özellikler
- **Giriş / Kayıt**: ASP.NET Core Identity ile e-posta + şifre tabanlı üyelik
- **Ürün Listesi**: Veritabanından (LocalDB) gelen örnek ürünler
- **Sepete Ürün Ekleme**: Sepet, oturum (Session) üzerinde JSON olarak tutulur
- **Sepeti Onaylama**: Giriş yapmış kullanıcı, teslimat adresi girip siparişi
  onaylar; sipariş veritabanına kaydedilir ve sepet temizlenir

## Gereksinimler
- Visual Studio 2019 (16.8 veya üzeri, ".NET 5.0" hedefleyebilmesi için)
- .NET 5.0 SDK (https://dotnet.microsoft.com/download/dotnet/5.0)
- SQL Server LocalDB (Visual Studio kurulumuyla genelde otomatik gelir)
- "ASP.NET ve web geliştirme" iş yükü Visual Studio Installer'dan kurulu olmalı

> Not: Visual Studio 2019, kutudan .NET 5'i desteklemez; en az 16.8 sürümü ve
> .NET 5 SDK'sının ayrıca kurulmuş olması gerekir.

## Kurulum Adımları

1. **Projeyi açın**: `CartMVCApp.csproj` dosyasına çift tıklayarak Visual
   Studio 2019'da açın (veya klasörü VS ile "Open Folder" ile açın).

2. **Paketleri geri yükleyin**: VS otomatik yapar; yapmazsa
   Package Manager Console'da:
   ```
   dotnet restore
   ```

3. **Veritabanını oluşturun** (migration + update):
   Package Manager Console'da (Tools > NuGet Package Manager > Package Manager Console):
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
   veya terminalden:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   (dotnet-ef aracı kurulu değilse: `dotnet tool install --global dotnet-ef`)

   Bu adım, Identity tablolarını ve örnek 6 ürünü (Seed Data) veritabanına
   ekler. Bağlantı dizesini kendi ortamınıza göre `appsettings.json` içinde
   değiştirebilirsiniz (varsayılan: `(localdb)\mssqllocaldb`).

4. **Projeyi çalıştırın**: F5 veya "IIS Express" / "CartMVCApp" profiliyle
   başlatın.

## Kullanım Akışı
1. Ana sayfadan **Ürünler** sayfasına gidin.
2. Bir ürünün yanındaki adet kutusunu ayarlayıp **Sepete Ekle** butonuna basın.
3. Üst menüden **Sepetim**'e girip ürünleri, adetlerini görüntüleyin/düzenleyin.
4. **Siparişi Onayla** butonuna basın (giriş yapılmamışsa otomatik olarak
   Giriş sayfasına yönlendirilirsiniz).
5. Teslimat adresinizi girip **Siparişi Onayla**'ya tekrar basın.
6. Sipariş numaranızı ve özetini gösteren onay sayfası açılır.

## Proje Yapısı
```
CartMVCApp/
├── Controllers/
│   ├── HomeController.cs
│   ├── AccountController.cs     (Giriş / Kayıt / Çıkış)
│   ├── ProductsController.cs    (Ürün listesi / detay)
│   └── CartController.cs        (Sepet işlemleri + Sipariş onaylama)
├── Models/
│   ├── ApplicationUser.cs       (Identity kullanıcı)
│   ├── Product.cs
│   ├── CartItem.cs              (Session'da tutulan sepet satırı)
│   ├── Order.cs / OrderItem.cs  (Onaylanan siparişler)
│   └── AccountViewModels.cs
├── Data/
│   └── ApplicationDbContext.cs  (EF Core + Identity + Seed Data)
├── Extensions/
│   └── SessionExtensions.cs     (Session'a JSON kaydetme/okuma)
├── Views/
│   ├── Home, Account, Products, Cart, Shared
└── wwwroot/css/site.css
```

## Ürün Görselleri
Ürün resimleri `wwwroot/images/` klasöründe yerel SVG dosyaları olarak
bulunur (internet bağlantısı gerekmez, dış servise bağımlı değildir).
Kendi ürün fotoğraflarınızı eklemek isterseniz:
1. Resim dosyanızı `wwwroot/images/` klasörüne kopyalayın.
2. `Data/ApplicationDbContext.cs` içindeki `ImageUrl` değerini
   `/images/dosyaadi.jpg` şeklinde güncelleyin (veya Admin panelinden/
   veritabanından ürün ekleyip oradan girin).

> Eğer daha önce `Update-Database` komutunu çalıştırdıysanız ve resimler
> hâlâ eski (bozuk) linkleri gösteriyorsa, veritabanını silip yeniden
> oluşturun:
> ```
> Drop-Database
> Update-Database
> ```

## Notlar / Genişletme Fikirleri
- Şu an şifre kuralları demo amaçlı gevşetilmiştir (Startup.cs içindeki
  `options.Password...` ayarları); üretimde daha güçlü kurallar önerilir.
- Ürün ekleme/düzenleme için basit bir Admin paneli eklenebilir.
- E-posta doğrulama, şifre sıfırlama gibi Identity özellikleri kolayca
  eklenebilir (`options.SignIn.RequireConfirmedAccount` vb.).
