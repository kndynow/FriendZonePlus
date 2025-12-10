import { FormControl, FormGroup, FormLabel } from "react-bootstrap";
interface FormFieldProps {
  label?: string;
  placeholder: string;
  type?: string;
  name?: string;
  value?: string;
  error?: string | null;
  touched?: boolean;
  required?: boolean;
  disabled?: boolean;
  rows?: number;
  onKeyDown?: (e: React.KeyboardEvent<HTMLInputElement>) => void;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
}
export default function FormField({
  label,
  placeholder,
  type = "text",
  name,
  value,
  error,
  touched,
  required = true,
  rows,
  onChange,
  onKeyDown,
  disabled,
}: FormFieldProps) {
  const isInvalid = Boolean(error && touched);

  return (
    <>
      <FormGroup className="mt-2 mb-1">
        {label && <FormLabel className="mb-2 fs-6">{label}</FormLabel>}

        <FormControl
          as={rows ? "textarea" : "input"}
          type={rows ? undefined : type}
          name={name}
          value={value}
          className="f-shadow-inset fs-6"
          placeholder={placeholder}
          onChange={onChange}
          onKeyDown={onKeyDown}
          required={required}
          disabled={disabled}
          isInvalid={isInvalid}
          maxLength={300}
        />

        {isInvalid && (
          <FormControl.Feedback type="invalid">{error}</FormControl.Feedback>
        )}
      </FormGroup>
    </>
  );
}
