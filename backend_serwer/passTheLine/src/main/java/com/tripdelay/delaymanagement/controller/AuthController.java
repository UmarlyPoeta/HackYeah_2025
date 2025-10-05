//package com.tripdelay.delaymanagement.controller;
//
//import com.tripdelay.delaymanagement.model.User;
//import com.tripdelay.delaymanagement.service.UserService;
//import org.springframework.beans.factory.annotation.Autowired;
//import org.springframework.http.ResponseEntity;
//import org.springframework.web.bind.annotation.*;
//
//@RestController
//@RequestMapping("/api/auth")
//public class AuthController {
//
//    @Autowired
//    private UserService userService;
//
//    @PostMapping("/register")
//    public ResponseEntity<User> register(@RequestBody User user) {
//        User savedUser = userService.registerUser(user);
//        return ResponseEntity.ok(savedUser);
//    }
//
//    // For login, perhaps use Spring Security, but for simplicity, assume JWT or basic
//    // This is basic, in real app use proper auth
//    @PostMapping("/login")
//    public String login(@RequestBody LoginRequest request) {
//        try {
//            Authentication auth = authenticationManager.authenticate(
//                    new UsernamePasswordAuthenticationToken(request.getUsername(), request.getPassword())
//            );
//            if (auth.isAuthenticated()) {
//                return "success";
//            }
//        } catch (AuthenticationException e) {
//            return "fail";
//        }
//        return "fail";
//    }
//
//    public static class LoginRequest {
//        private String username;
//        private String password;
//        public String getUsername() { return username; }
//        public void setUsername(String username) { this.username = username; }
//        public String getPassword() { return password; }
//        public void setPassword(String password) { this.password = password; }
//    }
//}

package com.tripdelay.delaymanagement.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/auth")
public class AuthController {

    @Autowired
    private AuthenticationManager authenticationManager;

    @PostMapping("/login")
    public String login(@RequestBody LoginRequest request) {
        try {
            Authentication auth = authenticationManager.authenticate(
                    new UsernamePasswordAuthenticationToken(request.getUsername(), request.getPassword())
            );
            if (auth.isAuthenticated()) {
                return "success";
            }
        } catch (AuthenticationException e) {
            return "fail";
        }
        return "fail";
    }

    public static class LoginRequest {
        private String username;
        private String password;
        public String getUsername() { return username; }
        public void setUsername(String username) { this.username = username; }
        public String getPassword() { return password; }
        public void setPassword(String password) { this.password = password; }
    }
}