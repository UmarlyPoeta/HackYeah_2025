# Agent Guidelines for HackYeah_2025

## Build Commands
- Build: `mvn clean compile`
- Run tests: `mvn test`
- Run single test: `mvn test -Dtest=TestClassName`
- Run application: `mvn spring-boot:run`

## Code Style Guidelines
- **Structure**: Standard Spring Boot (`controller/`, `service/`, `repository/`, `model/`, `config/`)
- **Packages**: `com.tripdelay.delaymanagement.{layer}`
- **Imports**: Jakarta persistence → Spring → Lombok (wildcard imports sparingly)
- **Naming**: Classes PascalCase, methods/variables camelCase, constants UPPER_SNAKE_CASE, DB tables snake_case
- **Annotations**: `@Autowired` injection, Lombok `@Data` for entities, `@RestController` with `/api/{resource}`
- **Services**: `@Service` with business logic, repositories extend `JpaRepository<Entity, ID>`
- **Error Handling**: `ResponseEntity<T>`, appropriate HTTP codes, `Optional<T>` for nullables
- **Types**: Primitives where possible, `@Column` for JPA, `@Valid` on request bodies
- **Security**: No sensitive logging, `PasswordEncoder` for passwords, Spring Security best practices
