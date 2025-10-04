package com.tripdelay.delaymanagement.controller;

import com.tripdelay.delaymanagement.model.Bus;
import com.tripdelay.delaymanagement.service.BusApiService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/buses")
public class BusController {

    @Autowired
    private BusApiService busApiService;

    // GET /api/buses - Pobierz wszystkie autobusy
    @GetMapping
    public ResponseEntity<List<Bus>> getAllBuses() {
        try {
            List<Bus> buses = busApiService.getAllBuses();
            return ResponseEntity.ok(buses);
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    // POST /api/buses - Dodaj nowy autobus
    @PostMapping
    public ResponseEntity<Bus> createBus(@RequestBody Bus bus) {
        try {
            Bus createdBus = busApiService.createBus(bus);
            return ResponseEntity.ok(createdBus);
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    // GET /api/buses/active - Pobierz aktywne autobusy
    @GetMapping("/active")
    public ResponseEntity<List<Bus>> getActiveBuses() {
        try {
            List<Bus> activeBuses = busApiService.getActiveBuses();
            return ResponseEntity.ok(activeBuses);
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    // GET /api/buses/{id} - Pobierz autobus po ID
    @GetMapping("/{id}")
    public ResponseEntity<Bus> getBusById(@PathVariable int id) {
        try {
            Bus bus = busApiService.getBusById(id);
            return ResponseEntity.ok(bus);
        } catch (Exception e) {
            return ResponseEntity.notFound().build();
        }
    }

    // PUT /api/buses/{id} - Aktualizuj autobus
    @PutMapping("/{id}")
    public ResponseEntity<Void> updateBus(@PathVariable int id, @RequestBody Bus bus) {
        try {
            busApiService.updateBus(id, bus);
            return ResponseEntity.ok().build();
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    // DELETE /api/buses/{id} - Usuń autobus
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteBus(@PathVariable int id) {
        try {
            busApiService.deleteBus(id);
            return ResponseEntity.ok().build();
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    // PUT /api/buses/{id}/location - Aktualizuj lokalizację
    @PutMapping("/{id}/location")
    public ResponseEntity<Void> updateBusLocation(@PathVariable int id, @RequestBody Bus.BusLocation location) {
        try {
            busApiService.updateBusLocation(id, location);
            return ResponseEntity.ok().build();
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    // PUT /api/buses/{id}/status - Aktualizuj status
    @PutMapping("/{id}/status")
    public ResponseEntity<Void> updateBusStatus(@PathVariable int id, @RequestBody BusStatusRequest statusRequest) {
        try {
            Bus.BusStatus status = Bus.BusStatus.fromValue(statusRequest.getStatus());
            busApiService.updateBusStatus(id, status);
            return ResponseEntity.ok().build();
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    // GET /api/buses/search - Wyszukaj po licensePlate
    @GetMapping("/search")
    public ResponseEntity<List<Bus>> searchBuses(@RequestParam String licensePlate) {
        try {
            List<Bus> buses = busApiService.searchBuses(licensePlate);
            return ResponseEntity.ok(buses);
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }

    // Klasa pomocnicza dla request body status update
    public static class BusStatusRequest {
        private int status;

        public int getStatus() {
            return status;
        }

        public void setStatus(int status) {
            this.status = status;
        }
    }
}