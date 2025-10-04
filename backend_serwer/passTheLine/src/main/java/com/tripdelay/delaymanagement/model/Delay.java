package com.tripdelay.delaymanagement.model;

import jakarta.persistence.*;
import lombok.Data;
import java.time.LocalDateTime;

@Entity
@Table(name = "delays")
@Data
public class Delay {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String route;

    private LocalDateTime scheduledTime;

    private LocalDateTime actualTime;

    private String reason;

    // Perhaps link to reports or something
}