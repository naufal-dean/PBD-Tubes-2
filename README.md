# Tanks: Extended

### IF3210-2021-Unity-K03-11

Permainan 3D _orthographic_ dengan gameplay _Tank Battle Royale_. Permainan ini merupakan ekstensi dari _Tanks - Unity Learn_ yang pernah dikerjakan pada tugas sebelumnya.

## Deskripsi Singkat Cara kerja

- Permainan dapat dimainkan _multiplayer_, secara _local area network_ dengan 4 orang pemain.
- Nama pemain dapat diatur pada _main menu_.
- Volume suara dapat diatur pada _main menu_.
- _Cash_ muncul secara periodik, yang dapat diambil oleh _tank_ untuk menambah _cash_ yang dimiliki.
- _Cash_ dapat digunakan untuk membeli senjata dan infantri.
- Tersedia tiga jenis senjata yang dapat digunakan oleh pemain:
  1. **_Tank Shell_**: Peluru shell dipengaruhi oleh gravitasi (bisa jatuh) dan meledak saat menabrak sesuatu. _Damage_ diukur berdasarkan jarak dari ledakan.
  2. **_Machine Gun_**: Senjata jarak jauh dan menembak dengan arah lurus serta tidak dipengaruhi oleh gravitasi. _Damage_-nya 5 dan memiliki _fire rate_ yang tinggi. Harganya $5/peluru.
  3. **_Shotgun_**: Senjata jarak dekat yang tidak terpengaruh oleh gravitasi dan _damage_ nya 20. Harganya $10/peluru.
- Tersedia dua jenis infantri yang dapat digunakan oleh pemain:
  1. **_Soldier_**: _Ranged unit_, menyerang dengan cara menembak, harganya $100.
  2. **_MobBear_**: _Melee unit_ seharga $50.
- Kendali:
  - W: Depan.
  - A: Kiri.
  - S: Belakang.
  - D: Kanan.
  - Q: Ganti senjata.
  - E: _Deploy_ infantri.
  - Spasi: Menembakkan senjata.

## Library dan Asset yang Digunakan

- **Mirror**: Untuk keperluan _multiplayer_. Tautan: https://assetstore.unity.com/packages/tools/network/mirror-129321.
- **Tanks Tutorial**: Tautan: https://assetstore.unity.com/packages/essentials/tutorial-projects/tanks-tutorial-46209?_ga=2.253809805.483014610.1616722566-364523867.1613917833
- **Survival Shooter Tutorial**: Tautan: https://learn.unity.com/project/survival-shooter-tutorial
- **Toon Soldiers WW2 Demo**: Untuk infantri jenis _soldier_. Tautan: https://assetstore.unity.com/packages/3d/characters/humanoids/toon-soldiers-ww2-demo-85702

## _Screenshot_ aplikasi

## Pembagian Kerja Anggota Kelompok

- 13518111 - Muhammad Mirza Fathan Al Arsyad:
  - Membuat _map_.
- 13518123 - Naufal Dean Anugrah:
- 13518135 - Gregorius Jovan Kresnadi:
  - Membuat senjata tambahan _Machine Gun_ dan _Shotgun_
  - Membuat kendali untuk ganti senjata, ganti infantri, deploy infantri
  - Membuat _Start Menu_, _Options Menu_, _Pause Menu_
  - Implementasi _Volume_ dan nama pemain dengan _PlayerPrevs_, dan peletakan nama di _Tank_
  - Membuat sistem _Cash_ dan pembelian peluru serta infantri
  - Membuat awal sistem _Object Pooling_
  - Modifikasi _map_
