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
  onChange,
  onKeyDown,
}: FormFieldProps) {
  const isInvalid = Boolean(error && touched);

  return (
    <>
      <FormGroup className="mt-2 mb-1">
        {label && <FormLabel className="mb-2 fs-6">{label}</FormLabel>}

        <FormControl
          type={type}
          name={name}
          value={value}
          className="f-shadow-inset fs-6"
          placeholder={placeholder}
          onChange={onChange}
          onKeyDown={onKeyDown}
          required={required}
          isInvalid={isInvalid}
        />

        {isInvalid && (
          <FormControl.Feedback type="invalid">{error}</FormControl.Feedback>
        )}
      </FormGroup>
    </>
  );
}
