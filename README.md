🕹️ MineSwapper
Klasická hra Minesweeper (Hledání min) implementovaná v jazyce C# (Windows Forms).
Hráč kliká na pole a snaží se odhalit všechny bezpečné buňky, aniž by narazil na minu.

##✨ Funkce
✅ Grafické rozhraní pomocí WinForms (klikání levým/pravým tlačítkem myši)
✅ Nastavitelné obtížnosti (Beginner, Advanced, Expert)
✅ Časový limit podle zvolené obtížnosti
✅ Zobrazení vlajek a čísel min v okolí
✅ Výhra/Prohra obrazovka přímo ve hře
✅ Načítání vlastních obrázků z adresáře images/
##📂 Struktura kódu
Projekt se skládá ze tří hlavních tříd:

###Form1
Hlavní UI třída (Windows Forms)
Řeší vykreslování herního pole (pictureBox1_Paint)
Zpracovává kliknutí myší (odhalení / vlajka)
Spravuje časovač a konec hry

###Game
Herní logika (vytváření mřížky, pokládání min, kontrola výhry, rekurzivní odhalování polí)
Obsahuje herní parametry pro různé obtížnosti

###Cell
Reprezentuje jednu buňku na hracím poli
Uchovává stav (hidden, revealed, flagged), souřadnice a informaci o mině

##🎮 Herní úrovně
Obtížnost	Rozměry mřížky	Počet min	Čas (s)
Beginner	7 × 7	5	60
Advanced	10 × 12	20	120
Expert	30 × 20	100	180

##🚀 Spuštění projektu
Naklonuj repozitář:
git clone https://github.com/RollEyes10/MineSwapper.git
Otevři projekt v Visual Studio (nebo jiném IDE podporujícím WinForms)
Ujisti se, že složka images/ obsahuje potřebné obrázky (dlaždice, mina, vlajka, pozadí, smajlík apod.)
Spusť aplikaci (F5)

📸 Ukázka hry


🔧 Možné rozšíření
Ukládání výsledků a žebříčku hráčů
Přidání custom obtížnosti (uživatelské nastavení velikosti a min)
Zvukové efekty při odhalení/odpálení miny
Lepší animace nebo modernější UI (např. pomocí WPF)
Opravit obrázky
