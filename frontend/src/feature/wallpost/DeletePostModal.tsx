import { Modal, Button } from "react-bootstrap";

type DeletePostModalProps = {
    show: boolean;
    onHide: () => void;
    onConfirm: () => void;
    isDeleting: boolean;
};

export default function DeletePostModal({
    show,
    onHide,
    onConfirm,
    isDeleting,
}: DeletePostModalProps) {
    return (
        <Modal show={show} onHide={() => !isDeleting && onHide()}>
            <Modal.Header closeButton>
                <Modal.Title>Delete Post</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p>Are you sure you want to delete this post? This action cannot be undone.</p>
            </Modal.Body>
            <Modal.Footer>
                <Button
                    variant="secondary"
                    onClick={onHide}
                    disabled={isDeleting}
                >
                    Cancel
                </Button>
                <Button
                    variant="danger"
                    onClick={onConfirm}
                    disabled={isDeleting}
                >
                    {isDeleting ? "Deleting..." : "Delete"}
                </Button>
            </Modal.Footer>
        </Modal>
    );
}

