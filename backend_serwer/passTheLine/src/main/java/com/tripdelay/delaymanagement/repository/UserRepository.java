package com.tripdelay.delaymanagement.repository;

import com.tripdelay.delaymanagement.model.User;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.Optional;

public interface UserRepository extends JpaRepository<User, Long> {

    Optional<User> findByUsername(String username);
}