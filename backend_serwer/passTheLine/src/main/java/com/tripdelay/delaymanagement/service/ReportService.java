package com.tripdelay.delaymanagement.service;

import com.tripdelay.delaymanagement.model.Report;
import com.tripdelay.delaymanagement.model.User;
import com.tripdelay.delaymanagement.repository.ReportRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;

@Service
public class ReportService {

    @Autowired
    private ReportRepository reportRepository;

    @Autowired
    private UserService userService;

    public Report createReport(Report report, String username) {
        User user = userService.findByUsername(username).orElseThrow(() -> new RuntimeException("User not found"));
        report.setUser(user);
        report.setTimestamp(LocalDateTime.now());
        return reportRepository.save(report);
    }

    public List<Report> getAllReports() {
        return reportRepository.findAll();
    }

    public List<Report> getVerifiedReports() {
        return reportRepository.findByVerified(true);
    }

    public void verifyReport(Long reportId) {
        Report report = reportRepository.findById(reportId).orElseThrow(() -> new RuntimeException("Report not found"));
        report.setVerified(true);
        reportRepository.save(report);
        // Update user reputation
        userService.updateReputation(report.getUser(), 10);
    }
}