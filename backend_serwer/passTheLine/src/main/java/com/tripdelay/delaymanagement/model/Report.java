package com.tripdelay.delaymanagement.model;

import jakarta.persistence.*;
import lombok.Data;
import java.time.LocalDateTime;

@Entity
@Table(name = "reports")
@Data
public class Report {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    @Enumerated(EnumType.STRING)
    private ReportType type; // DELAY, DISRUPTION

    private String description;

    private String location; // Perhaps latitude,longitude or address

    private LocalDateTime timestamp;

    private boolean verified = false;

    private int votes = 0;

    // Add route or line info if needed
}

enum ReportType {
    DELAY, DISRUPTION
}