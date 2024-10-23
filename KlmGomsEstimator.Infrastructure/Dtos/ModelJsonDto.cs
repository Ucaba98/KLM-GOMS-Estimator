namespace KlmGomsEstimator.Infrastructure.Dtos;

public record ModelJsonDto
(
    string Description,
    List<InstructionJsonDto> Instructions
);

public record InstructionJsonDto
(
    string Description,
    List<StepJsonDto> Steps
);

public record StepJsonDto
(
    string Description,
    string OperatorCode
);
