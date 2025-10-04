package com.tripdelay.delaymanagement.model;

import lombok.Data;

@Data
public class Bus {
    private int id;
    private String licensePlate;
    private String model;
    private int capacity;
    private String busNumber;
    private BusLocation location;
    private BusStatus status;

    @Data
    public static class BusLocation {
        private double latitude;
        private double longitude;
        private double speed;
        private double bearing;
    }

    public enum BusStatus {
        INACTIVE(0),
        ACTIVE(1),
        MAINTENANCE(2),
        OUT_OF_SERVICE(3);

        private final int value;

        BusStatus(int value) {
            this.value = value;
        }

        public int getValue() {
            return value;
        }

        public static BusStatus fromValue(int value) {
            for (BusStatus status : BusStatus.values()) {
                if (status.value == value) {
                    return status;
                }
            }
            throw new IllegalArgumentException("Unknown status value: " + value);
        }
    }
}