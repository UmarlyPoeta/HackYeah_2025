# Agent Guidelines for HackYeah_2025

## Build Commands
- Build: `mvn clean compile`
- Run tests: `mvn test`
- Run single test: `mvn test -Dtest=TestClassName`
- Run application: `mvn spring-boot:run`

## Environment
- Java 17, Spring Boot 3.5.6, Lombok, H2 database
- Testing: JUnit 5 with Spring Boot Test

## Code Style Guidelines
- **Structure**: Standard Spring Boot layers (`controller/`, `service/`, `repository/`, `model/`, `config/`)
- **Packages**: `com.tripdelay.delaymanagement.{layer}`
- **Imports**: Jakarta persistence → Spring → Lombok (wildcard imports sparingly)
- **Naming**: PascalCase classes, camelCase methods/variables, UPPER_SNAKE_CASE constants, snake_case DB tables
- **Annotations**: `@Autowired` injection, `@Data` for entities, `@RestController` with `/api/{resource}`
- **Services**: `@Service` with business logic, repositories extend `JpaRepository<Entity, ID>`
- **Error Handling**: `ResponseEntity<T>`, HTTP codes, `Optional<T>` for nullables
- **Types**: Primitives where possible, `@Column` for JPA, `@Valid` on request bodies
- **Security**: No sensitive logging, `PasswordEncoder` for passwords, Spring Security best practices
