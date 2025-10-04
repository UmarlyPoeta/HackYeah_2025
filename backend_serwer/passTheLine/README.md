# 🚇 Delay Management System

System do zarządzania opóźnieniami transportu publicznego - aplikacja webowa pozwalająca pasażerom zgłaszać problemy z komunikacją miejską.

## 📋 Wymagania

- **Java**: 17 lub nowszy
- **Maven**: 3.6+
- **Przeglądarka internetowa**

## 🚀 Uruchomienie aplikacji

### Krok 1: Uruchom aplikację
```bash
cd passTheLine
mvn spring-boot:run
```

### Krok 2: Sprawdź czy aplikacja działa
Otwórz przeglądarkę i przejdź do: `http://localhost:8081`

Powinieneś zobaczyć przekierowanie do strony głównej.

## 📱 Jak korzystać z aplikacji

### Krok 1: Otwórz stronę główną
Przejdź do: `http://localhost:8081`

### Krok 2: Przejdź do strony zgłaszania problemów
Kliknij link do strony reportowania lub przejdź bezpośrednio do:
`http://localhost:8081/report-page.html`

### Krok 3: Wypełnij formularz zgłoszenia
Wypełnij następujące pola:
- **Numer linii** (np. 5, 10, 15)
- **Poziom zatłoczenia**: LOW / MEDIUM / HIGH / URGENT
- **Opóźnienie w minutach** (np. 10, 20, 30)
- **Awaria pojazdu** (opcjonalne): TEMPORARY / COMPLETE
- **Klimatyzacja** (opcjonalne): WORKING / NOT_WORKING
- **Zapach** (opcjonalne): NORMAL / BAD

### Krok 4: Wyślij zgłoszenie
Kliknij przycisk "Wyślij zgłoszenie".

### Krok 5: Sprawdź potwierdzenie
Jeśli zgłoszenie zostało przyjęte, zobaczysz komunikat potwierdzający.

## 🔍 Jak sprawdzić czy zgłoszenie zostało przesłane

### Metoda 1: Sprawdź odpowiedź API (zalecane)
Po wysłaniu formularza, API zwraca potwierdzenie w formacie JSON z ID zgłoszenia.

### Metoda 2: Sprawdź wszystkie zgłoszenia
```bash
curl http://localhost:8081/api/reports
```

### Metoda 3: Sprawdź bazę danych przez H2 Console
1. Otwórz: `http://localhost:8081/h2-console`
2. **JDBC URL**: `jdbc:h2:mem:testdb`
3. **Username**: `sa`
4. **Password**: `password`
5. Wykonaj zapytanie: `SELECT * FROM reports;`

## 📡 Dostępne endpointy API

### Zgłaszanie problemów
```bash
POST /api/reports/submit?lineNumber=5&crowdLevel=HIGH&delayMinutes=20
```

### Pobieranie wszystkich zgłoszeń
```bash
GET /api/reports
```

### Dostęp do stron HTML
- `http://localhost:8081/index.html` - Strona główna
- `http://localhost:8081/report-page.html` - Formularz zgłoszeń
- `http://localhost:8081/login.html` - Logowanie
- `http://localhost:8081/profile.html` - Profil użytkownika

## 🛠 Rozwiązywanie problemów

### Problem: Port 8081 jest zajęty
```bash
# Znajdź proces używający portu 8081
lsof -ti:8081

# Zatrzymaj proces
kill -9 <PID>
```

### Problem: Aplikacja nie uruchamia się
```bash
# Wyczyść i przebuduj projekt
mvn clean compile

# Uruchom ponownie
mvn spring-boot:run
```

### Problem: Baza danych jest pusta
Baza H2 jest in-memory, więc po restarcie aplikacji wszystkie dane są tracone. To normalne zachowanie.

## 📊 Dostępne wartości w formularzach

### Poziom zatłoczenia (CrowdLevel)
- `LOW` - Małe zatłoczenie
- `MEDIUM` - Średnie zatłoczenie
- `HIGH` - Duże zatłoczenie
- `URGENT` - Krytyczne zatłoczenie

### Awaria pojazdu (VehicleFailure)
- `TEMPORARY` - Tymczasowa awaria
- `COMPLETE` - Całkowita awaria

### Klimatyzacja (AirConditioning)
- `WORKING` - Działa
- `NOT_WORKING` - Nie działa

### Zapach (Smell)
- `NORMAL` - Normalny
- `BAD` - Nieprzyjemny

## 🎯 Przykład użycia

```bash
# 1. Uruchom aplikację
mvn spring-boot:run

# 2. Wyślij zgłoszenie przez API
curl -X POST "http://localhost:8081/api/reports/submit?lineNumber=5&crowdLevel=HIGH&delayMinutes=20&vehicleFailure=TEMPORARY"

# 3. Sprawdź wszystkie zgłoszenia
curl http://localhost:8081/api/reports
```

## 📞 Kontakt

W przypadku problemów lub pytań dotyczących aplikacji, sprawdź logi aplikacji w pliku `app.log` lub zgłoś issue w repozytorium.