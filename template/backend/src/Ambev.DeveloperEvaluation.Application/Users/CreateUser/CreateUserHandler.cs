﻿using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser
{
    /// <summary>
    /// Handler for processing CreateUserCommand requests
    /// </summary>
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<CreateUserHandler> _logger;

        /// <summary>
        /// Initializes a new instance of CreateUserHandler
        /// </summary>
        /// <param name="userRepository">The user repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="passwordHasher">The password hasher</param>
        /// <param name="logger">The logger</param>
        public CreateUserHandler(IUserRepository userRepository,
                                 IMapper mapper, 
                                 IPasswordHasher passwordHasher, 
                                 ILogger<CreateUserHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <summary>
        /// Handles the CreateUserCommand request
        /// </summary>
        /// <param name="command">The CreateUser command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created user details</returns>
        public async Task<CreateUserResult> Handle(CreateUserCommand command, 
                                                   CancellationToken cancellationToken)
        {
            var validator = new CreateUserCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateUserCommand. Errors: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingUser != null)
            {
                _logger.LogWarning("Attempt to create a user with an existing email: {Email}", command.Email);
                throw new InvalidOperationException($"User with email {command.Email} already exists");
            }

            var user = _mapper.Map<User>(command);
            user.Password = _passwordHasher.HashPassword(command.Password);

            var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
            var result = _mapper.Map<CreateUserResult>(createdUser);

            _logger.LogInformation("User created successfully with email: {Email}", command.Email);

            return result;
        }
    }
}
