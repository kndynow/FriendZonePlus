import type { RegisterRequest } from "../types/auth";

// Returns an object of errors, keyed by field name
export function validateRegister(values: RegisterRequest) {
  const errors: Partial<Record<keyof RegisterRequest, string>> = {};

  // Username
  if (!values.username) errors.username = "Username is required";
  else if (values.username.length < 3)
    errors.username = "Username must be at least 3 characters";
  else if (values.username.length > 20)
    errors.username = "Username cannot exceed 20 characters";

  // Email
  if (!values.email) errors.email = "Email is required";
  else if (values.email.length < 5)
    errors.email = "Email must be longer than 5 characters";
  else if (values.email.length > 50)
    errors.email = "Email cannot exceed 50 characters";
  else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(values.email))
    errors.email = "Invalid email";

  // Password
  if (!values.password) errors.password = "Password is required";
  else if (values.password.length < 6)
    errors.password = "Password must be longer than 6 characters";
  else if (values.password.length > 30)
    errors.password = "Password cannot be longer than 30 characters";
  else if (!/[A-Z]/.test(values.password))
    errors.password = "Password must contain at least one uppercase letter";
  else if (!/[a-z]/.test(values.password))
    errors.password = "Password must contain at least one lowercase letter";
  else if (!/[0-9]/.test(values.password))
    errors.password = "Password must contain at least one number";

  // First name
  if (!values.firstName) errors.firstName = "First name is required";
  else if (values.firstName.length > 30)
    errors.firstName = "First name cannot exceed 30 characters";
  else if (!/^\p{L}+$/u.test(values.firstName))
    errors.firstName = "First name can only contain letters";

  // Last name
  if (!values.lastName) errors.lastName = "Last name is required";
  else if (values.lastName.length > 30)
    errors.lastName = "Last name cannot exceed 30 characters";
  else if (!/^[\p{L}-]+$/u.test(values.lastName))
    errors.lastName = "Last name can only contain letters and hyphens";

  return errors;
}
