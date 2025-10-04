package com.tripdelay.delaymanagement.repository;

import com.tripdelay.delaymanagement.model.Delay;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface DelayRepository extends JpaRepository<Delay, Long> {

    List<Delay> findByRoute(String route);
}