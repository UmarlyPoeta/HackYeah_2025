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

    // New fields for the HTML form
    private Integer lineNumber; // Numer linii

    @Enumerated(EnumType.STRING)
    private CrowdLevel crowdLevel; // Poziom zatłoczenia

    private Integer delayMinutes; // Opóźnienie w minutach

    @Enumerated(EnumType.STRING)
    private VehicleFailure vehicleFailure; // Awaria pojazdu

    @Enumerated(EnumType.STRING)
    private AirConditioning airConditioning; // Klimatyzacja

    @Enumerated(EnumType.STRING)
    private Smell smell; // Wrażenia zapachowe
}