package com.tripdelay.delaymanagement.controller;

import com.tripdelay.delaymanagement.model.Delay;
import com.tripdelay.delaymanagement.service.DelayService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/delays")
public class DelayController {

    @Autowired
    private DelayService delayService;

    @PostMapping
    public ResponseEntity<Delay> createDelay(@RequestBody Delay delay) {
        Delay savedDelay = delayService.createDelay(delay);
        return ResponseEntity.ok(savedDelay);
    }

    @GetMapping
    public ResponseEntity<List<Delay>> getDelays(@RequestParam(required = false) String route) {
        List<Delay> delays;
        if (route != null) {
            delays = delayService.getDelaysByRoute(route);
        } else {
            delays = delayService.getAllDelays();
        }
        return ResponseEntity.ok(delays);
    }
}