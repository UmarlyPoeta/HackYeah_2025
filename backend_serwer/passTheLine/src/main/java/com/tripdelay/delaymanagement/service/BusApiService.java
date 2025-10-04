package com.tripdelay.delaymanagement.service;

import com.tripdelay.delaymanagement.model.Bus;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.ParameterizedTypeReference;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.List;
import java.util.Map;

import java.util.List;

@Service
public class BusApiService {

    private final RestTemplate restTemplate;
    private final String baseUrl;

    public BusApiService(@Value("${external.api.base-url:http://192.168.137.1:5041}") String baseUrl) {
        this.restTemplate = new RestTemplate();
        this.baseUrl = baseUrl;
    }

    // GET /api/Buses - Pobierz wszystkie autobusy
    public List<Bus> getAllBuses() {
        String url = baseUrl + "/api/Buses";
        ResponseEntity<List<Bus>> response = restTemplate.exchange(
            url,
            HttpMethod.GET,
            null,
            new ParameterizedTypeReference<List<Bus>>() {}
        );
        return response.getBody();
    }

    // POST /api/Buses - Dodaj nowy autobus
    public Bus createBus(Bus bus) {
        String url = baseUrl + "/api/Buses";
        return restTemplate.postForObject(url, bus, Bus.class);
    }

    // GET /api/Buses/active - Pobierz aktywne autobusy
    public List<Bus> getActiveBuses() {
        String url = baseUrl + "/api/Buses/active";
        ResponseEntity<List<Bus>> response = restTemplate.exchange(
            url,
            HttpMethod.GET,
            null,
            new ParameterizedTypeReference<List<Bus>>() {}
        );
        return response.getBody();
    }

    // GET /api/Buses/{id} - Pobierz autobus po ID
    public Bus getBusById(int id) {
        String url = baseUrl + "/api/Buses/" + id;
        return restTemplate.getForObject(url, Bus.class);
    }

    // PUT /api/Buses/{id} - Aktualizuj autobus
    public void updateBus(int id, Bus bus) {
        String url = baseUrl + "/api/Buses/" + id;
        restTemplate.put(url, bus);
    }

    // DELETE /api/Buses/{id} - Usuń autobus
    public void deleteBus(int id) {
        String url = baseUrl + "/api/Buses/" + id;
        restTemplate.delete(url);
    }

    // PUT /api/Buses/{id}/location - Aktualizuj lokalizację
    public void updateBusLocation(int id, Bus.BusLocation location) {
        String url = baseUrl + "/api/Buses/" + id + "/location";
        restTemplate.put(url, location);
    }

    // PUT /api/Buses/{id}/status - Aktualizuj status
    public void updateBusStatus(int id, Bus.BusStatus status) {
        String url = baseUrl + "/api/Buses/" + id + "/status";
        restTemplate.put(url, Map.of("status", status.getValue()));
    }

    // GET /api/Buses/search - Wyszukaj po licensePlate
    public List<Bus> searchBuses(String licensePlate) {
        String url = baseUrl + "/api/Buses/search?licensePlate=" + licensePlate;
        ResponseEntity<List<Bus>> response = restTemplate.exchange(
            url,
            HttpMethod.GET,
            null,
            new ParameterizedTypeReference<List<Bus>>() {}
        );
        return response.getBody();
    }
}