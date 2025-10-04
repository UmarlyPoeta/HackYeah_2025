package com.tripdelay.delaymanagement.controller;

import com.tripdelay.delaymanagement.model.User;
import com.tripdelay.delaymanagement.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/auth")
public class AuthController {

    @Autowired
    private UserService userService;

    @PostMapping("/register")
    public ResponseEntity<User> register(@RequestBody User user) {
        User savedUser = userService.registerUser(user);
        return ResponseEntity.ok(savedUser);
    }

    // For login, perhaps use Spring Security, but for simplicity, assume JWT or basic
    // This is basic, in real app use proper auth
}