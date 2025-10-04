package com.tripdelay.delaymanagement;

import com.tripdelay.delaymanagement.model.User;
import com.tripdelay.delaymanagement.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

@SpringBootApplication
public class DelayManagementApplication {

	public static void main(String[] args) {
		SpringApplication.run(DelayManagementApplication.class, args);
	}

	@Bean
	public CommandLineRunner createAnonymousUser(UserService userService) {
		return args -> {
			if (userService.findByUsername("anonymous").isEmpty()) {
				User anonymous = new User();
				anonymous.setUsername("anonymous");
				anonymous.setPassword("anonymous"); // This will be encoded
				userService.registerUser(anonymous);
			}
		};
	}

}
