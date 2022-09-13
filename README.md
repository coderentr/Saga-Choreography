# `SAGA Pattern`

Bu paternde her service bağımsız bir service üzerinde çalıştığı için o service üzerinde verinin güncellendiği transectionlardır. 
Bu yönteme göre transection dış etki ile (butone basma vs) tetiklenir ve artık sonraki tüm transectionlar bir önceki transectionun başarılı durumuna göre devam eder. 
Herhangi bir transectionda meydana gelecek bir hata ile tüm transectionlar iptal edilerek Atomicity  prensibine bağlılık saplanmış olunur. 
Sagayı uygulamak için 2 farklı yöntem vardır. Bunlar __Event/Choreography__ ve __Commend/Orchestration__ dur. 

- __Event/Choreography__ 
Bu yöntem ile bir transection event fırlatır ve diğer transectionlar evente göre kendi locallerinde çalışır. Her service bir öncekinin işini bitirip haber vermesini 
bekler.  Son transection gerçekleştiğinde işlem sonlandırılmış olur. 
Tramsectionlar birbirlerinden izoledir ve birbirleri hakkında bilgi sahibi olmak zorunda değildirler. 
Serviceler ve transectionlar arttıkça yönetilebilirliği zorlaşmaktadır. 

__Senaryo__; 
- Sipariş Servisi çalıştırılır.
- Sipariş oluşturuldu eventi fırlatılır. 
- Bu eventi dinleyen stok servisi tetiklenir. 
- Stok yeterli ise ödeme servisine event fırlatılır. Stok yatersiz ise sipariş servisine event fırlatılıp siparişin durumu güncellenir. 
- Ödeme servisinden ödeme alındı yada hata eventi fırlatılır. 
- Ödeme başarılı ise sipariş serviste siparişin durumu tamamlandıya çekilir. 
- Ödemenin başarısız olması durumunda Stok service ve Sipariş servise event fırlatılır. Stok eski haline getirilip siparişin durumu güncellenir. 

