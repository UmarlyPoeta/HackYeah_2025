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

    // New endpoint for creating reports from the HTML form
    @PostMapping("/submit")
    public ResponseEntity<Report> submitProblemReport(
            @RequestParam(required = false) Integer lineNumber,
            @RequestParam(required = false) String crowdLevel,
            @RequestParam(required = false) Integer delayMinutes,
            @RequestParam(required = false) String vehicleFailure,
            @RequestParam(required = false) String airConditioning,
            @RequestParam(required = false) String smell) {

        String username = "anonymous";
        Report report = new Report();

        // Set the form data
        report.setLineNumber(lineNumber);
        if (crowdLevel != null) {
            report.setCrowdLevel(com.tripdelay.delaymanagement.model.CrowdLevel.valueOf(crowdLevel.toUpperCase()));
        }
        report.setDelayMinutes(delayMinutes);
        if (vehicleFailure != null) {
            report.setVehicleFailure(com.tripdelay.delaymanagement.model.VehicleFailure.valueOf(vehicleFailure.toUpperCase()));
        }
        if (airConditioning != null) {
            report.setAirConditioning(com.tripdelay.delaymanagement.model.AirConditioning.valueOf(airConditioning.toUpperCase()));
        }
        if (smell != null) {
            report.setSmell(com.tripdelay.delaymanagement.model.Smell.valueOf(smell.toUpperCase()));
        }

        // Set default type as DISRUPTION for problem reports
        report.setType(com.tripdelay.delaymanagement.model.ReportType.DISRUPTION);

        // Create description from the form data
        StringBuilder description = new StringBuilder();
        if (lineNumber != null) description.append("Linia ").append(lineNumber).append(". ");
        if (crowdLevel != null) description.append("Zatłoczenie: ").append(crowdLevel).append(". ");
        if (delayMinutes != null) description.append("Opóźnienie: ").append(delayMinutes).append(" min. ");
        if (vehicleFailure != null) description.append("Awaria: ").append(vehicleFailure).append(". ");
        if (airConditioning != null) description.append("Klimatyzacja: ").append(airConditioning).append(". ");
        if (smell != null) description.append("Zapach: ").append(smell).append(".");
        report.setDescription(description.toString().trim());

        Report savedReport = reportService.createReport(report, username);
        return ResponseEntity.ok(savedReport);
    }
}