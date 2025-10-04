package com.tripdelay.delaymanagement.repository;

import com.tripdelay.delaymanagement.model.Report;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface ReportRepository extends JpaRepository<Report, Long> {

    List<Report> findByVerified(boolean verified);

    List<Report> findByLocation(String location);
}