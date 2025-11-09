<!--
  README dla projektu Warehouse-App
  Język: polski
-->
# 🏬 Warehouse-App — Aplikacja do obsługi magazynu ✨

📝 Licencja: MIT — szczegóły w pliku `LICENSE`
## Najważniejsze informacje 🔎

- Nazwa projektu: Warehouse-App (lokalnie: `WpfApp1`)
- Typ aplikacji: Desktop (WPF, XAML)
- Język: C#
- Paradygmat: MVVM (widoki w XAML + ViewModel-e w C#)
- Cel: obsługa operacji magazynowych — inwentaryzacja, przyjęcia, wydania, monitoring stanów

## Krótki opis ✨

Warehouse-App to aplikacja zaprojektowana do obsługi procesów magazynowych dla małych i średnich przedsiębiorstw. Interfejs użytkownika wykonany jest w WPF z zastosowaniem wzorca MVVM, co ułatwia separację logiki biznesowej od prezentacji i jej testowanie.

Główne funkcje (obecne lub planowane):

- Przegląd i zarządzanie stanami magazynowymi
- Rejestracja przyjęć i wydań towarów
- Widoki modułowe (Page1…Page6) oraz mierniki/indikatory w UI
- Łatwe punkty rozszerzeń: integracja z DB, skanery kodów kreskowych, eksport raportów

## Stos technologiczny 🛠️

- Platforma: .NET / WPF (Windows desktop)
- Język: C#
- UI: XAML (WPF)
- Wzorzec: MVVM (przykładowe pliki: `batteryviewmodel.cs`, `GaugeViewModel.cs`)
- Narzędzia: Visual Studio (zalecane), Git

Uwaga: projekt jest przeznaczony na Windows; do kompilacji najlepiej użyć Visual Studio z obsługą WPF.



## Scenariusze zastosowań 💼

- Zarządzanie zapasami centralnego/oddziałowego magazynu
- Rejestracja przyjęć od dostawców oraz wydania na zlecenia
- Przeglądy stanów i alarmy o niskim stanie (rozszerzenie)

## Instrukcja uruchomienia ▶️

Otwórz `WpfApp1.sln` w Visual Studio i skompiluj projekt (Build -> Build Solution). Uruchom aplikację w trybie Debug/Release.

Alternatywnie (jeśli projekt wspiera dotnet CLI):

```powershell
# W katalogu repozytorium (z WpfApp1.sln)
dotnet build WpfApp1.sln
dotnet run --project .\WpfApp1\WpfApp1.csproj
```

Wskazówki:
- Jeśli pojawią się błędy związane z brakującym SDK lub pakietami WPF, uruchom Visual Studio i zainstaluj sugerowane komponenty.
- Przy błędach XAML sprawdź ResourceDictionary i właściwości Build Action plików zasobów.

## Proponowane rozszerzenia / roadmapa 🚀

1. Integracja z bazą danych (SQLite / SQL Server) i warstwą repozytorium
2. Autoryzacja i role (magazynier, menedżer)
3. Obsługa skanerów kodów kreskowych / czytników RFID
4. Raportowanie (CSV, PDF) i eksport danych
5. Testy jednostkowe dla ViewModeli i testy integracyjne

## Checklist testów manualnych ✅

- Uruchom aplikację i przejdź przez dostępne strony — czy UI działa bez wyjątków?
- Dodaj przykładowe przyjęcie towaru — czy stan magazynowy aktualizuje się?
- Przeprowadź wydanie towaru — czy logika zmiany stanu działa poprawnie?

## Contributing 🤝

1. Utwórz branch `feature/krótki-opis`
2. Dodaj/edytuj testy
3. Wyślij pull request z opisem zmian

## Licencja 📝

Projekt jest udostępniony na licencji MIT. Pełny tekst licencji znajduje się w pliku `LICENSE` w katalogu głównym repozytorium.