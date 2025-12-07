# FightStitch ( Echipa 9 )

Acest document ofera instructiunile esentiale pentru a rula si intelege acest proiect.

## Cum se ruleaza proiectul

1.  Clonati repository-ul local folosind Git:
    `git clone https://github.com/DoubleF9/FightingGame`
2.  Deschideti aplicatia Unity Hub.
3.  Faceti clic pe "Open" (sau "Add project from disk").
4.  Navigati la directorul unde ati clonat proiectul si selectati-l.
5.  Verificati daca aveti instalata versiunea de Unity corespunzatoare proiectului.
6.  Dupa ce proiectul se incarca in Unity Editor, mergeti in fereastra "Project", la `Assets -> Scenes`.
7.  Acolo se afla Map1, Map2, Map3. Acestea sunt scenele care vor putea fi alese de catre jucator ca background pentru lupta.
8.  Intrati pe Map2 si dati play. Pentru a alege personajul vostru si inamicul, dati toggle active in inspector la un Player si un Opponent din hierarchy. Controlati personajul din stanga si va luptati cu cel din dreapta.


Acest repository găzduiește proiectul Unity pentru jocul nostru de tip "fighting game", inspirat de titluri precum Mortal Kombat.

## Stadiul Proiectului (Final Sprint 2)

La momentul actual, proiectul include următoarele funcționalități și resurse:

### 🛠️ Configurare și Structură
* Structura de bază a folderelor (Scene, Scripturi, Prefabs etc.).
* Configurarea inițială a proiectului și fișierul `.gitignore`.

### 🌍 Environment & Artă
* **3 Scene de background** cu modele 3D și collidere, din care jucătorul va putea alege.
* Un model 3D și un rig complet pentru primul personaj.

### 🎮 Gameplay & Controale
* **Movement:** Controlul personajului se face folosind tastele **WASD**.
* **Sistem de Atac:** Există 4 tipuri de atacuri mapate pe tastele **1, 2, 3, 4**.
* **Dodge:** Mecanică de eschivă implementată pe tasta **E**.
* **Camera:** Script de cameră (Camera Follow) care urmărește automat mișcarea jucătorului.

### ⚔️ Sistem de Luptă (Combat System)
* **Health & Damage:** Sistem funcțional de viață și damage.
    * *Notă:* Momentan nu există un HealthBar vizual (UI), dar logica este activă.Nu avem nici animatii in momentul acesta.
* **Stare de deces:** Când viața unui personaj ajunge la 0, acesta devine inactiv (dispare din scenă).
* **Feedback:**
    * Efecte sonore (Sound Effects) la impact.
    * Debug log-uri în consolă pentru a monitoriza cine primește damage și statusul luptei.

### 🤖 AI
* Un inamic de bază (Basic AI Fighter) care:
    * Urmărește jucătorul prin scenă.
    * Poate ataca și încasa damage.

## Rapoarte Sprint

Toate rapoartele de progres pentru fiecare sprint pot fi gasite in folderul "Sprints" din acest repository.

## Recenzie

Modalitatea preferata pentru a primi feedback (raportul de recenzie) este prin e-mail, la adresa: **turcuianis@gmail.com**.
