# Saga-Choreography

Saga-Choreography yöntemi ile sipariş simülasyonu. 

Sipariş Servisine gelen bir talebin alınıp stok servisinde stok kontrolü yapıldıktan sonra ödeme işleminin yapılması süreci geliştirilmesi yapılmıştır.

Sipariş alındıktan sonra event fırlatılmakta ve stok servis bu eventi dinlemekte, gelen mesajdaki sipariş ve sipariş detay bilgilerine göre sipariş edilen ürün stokta varsa stoktan düşülüp ödeme servisine event fırlatılmakta, ürün stokta yoksa sipariş servisindeki işlemlerin geri alınması için yetersiz stok bilgisi sipatiş servise dönülmektedir.\
Ödeme servisinde ödeme başarılı bir şekilde alındıysa sipariş servisine başarılı mesajı dönülüp ürünün durumu güncellenmekte, ödemede problem olması halinde stok ve sipariş servislerinin dinlediği cunsomerlara event fırlatılmakta, stok serviste yapılan değişiklikler geri alınıp sipariş serviste sipatirin durumu güncellenmektedir.
