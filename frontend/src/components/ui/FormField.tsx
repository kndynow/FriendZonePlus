import { FormControl, FormGroup, FormLabel } from "react-bootstrap";
interface FormFieldProps {
  label?: string;
  placeholder: string;
  type?: string;
  name?: string;
  value?: string;
  onKeyDown?: (e: React.KeyboardEvent<HTMLInputElement>) => void;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
}
export default function FormField({
  label,
  placeholder,
  type = "text",
  name,
  value,
  onChange,
  onKeyDown,
}: FormFieldProps) {
  return (
    <>
      <FormGroup>
        {label && <FormLabel>{label}</FormLabel>}
        <FormControl
          type={type}
          name={name}
          value={value}
          onChange={onChange}
          className="f-shadow-inset"
          placeholder={placeholder}
          onKeyDown={onKeyDown}
          required
        />
      </FormGroup>
    </>
  );
}
