package com.tripdelay.delaymanagement.controller;

import com.tripdelay.delaymanagement.model.Report;
import com.tripdelay.delaymanagement.service.ReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/reports")
public class ReportController {

    @Autowired
    private ReportService reportService;

    @PostMapping
    public ResponseEntity<Report> createReport(@RequestBody Report report, Authentication authentication) {
        String username = authentication.getName();
        Report savedReport = reportService.createReport(report, username);
        return ResponseEntity.ok(savedReport);
    }

    @GetMapping
    public ResponseEntity<List<Report>> getReports() {
        List<Report> reports = reportService.getAllReports();
        return ResponseEntity.ok(reports);
    }

    @PostMapping("/{id}/verify")
    public ResponseEntity<Void> verifyReport(@PathVariable Long id) {
        reportService.verifyReport(id);
        return ResponseEntity.ok().build();
    }
}