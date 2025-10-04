package com.tripdelay.delaymanagement.service;

import com.tripdelay.delaymanagement.model.Delay;
import com.tripdelay.delaymanagement.repository.DelayRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class DelayService {

    @Autowired
    private DelayRepository delayRepository;

    public Delay createDelay(Delay delay) {
        return delayRepository.save(delay);
    }

    public List<Delay> getDelaysByRoute(String route) {
        return delayRepository.findByRoute(route);
    }

    public List<Delay> getAllDelays() {
        return delayRepository.findAll();
    }
}