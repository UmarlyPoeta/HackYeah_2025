package com.tripdelay.delaymanagement.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class PageController {

    @GetMapping("/")
    public String index() {
        return "redirect:/index.html";
    }

    @GetMapping("/login")
    public String login() {
        return "redirect:/login.html";
    }

    @GetMapping("/main")
    public String mainPage() {
        return "redirect:/main_page.html";
    }

    @GetMapping("/report")
    public String reportPage() {
        return "redirect:/report-page.html";
    }

    @GetMapping("/profile")
    public String profile() {
        return "redirect:/profile.html";
    }

    @GetMapping("/station")
    public String busStation() {
        return "redirect:/bus_station.html";
    }

    @GetMapping("/lines")
    public String lineList() {
        return "redirect:/line-list.html";
    }

    @GetMapping("/map")
    public String mapa() {
        return "redirect:/mapa.html";
    }
}