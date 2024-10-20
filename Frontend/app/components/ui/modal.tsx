import React, { ReactNode } from 'react';
import ReactDOM from 'react-dom';

import styles from './assets/model.module.css'; // Assuming you are using CSS modules

const modalHeader = styles.modalHeader; // Define modalHeader from styles

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  children: ReactNode;
  title?: string;
  closeOnOverlayClick?: boolean; // Add the new prop
}

const Modal: React.FC<ModalProps> = ({ isOpen, onClose, children, title, closeOnOverlayClick = true }) => {
  if (!isOpen) return null;

  const handleOverlayClick = (e: React.MouseEvent) => {
    if (closeOnOverlayClick && e.target === e.currentTarget) {
      onClose();
    }
  };

  return ReactDOM.createPortal(
    <div className={styles.modalOverlay} onClick={handleOverlayClick}>
      <div className={styles.modalContent} style={{ backgroundColor: '#f0f0f0', border: '1px solid #ccc' }}>
        <div className={modalHeader}>
          {title && <h2 className={styles.modalTitle}>{title}</h2>}
          <button className={styles.closeButton} onClick={onClose}>
            &times;
          </button>
        </div>
        <div className={styles.modalBody}>{children}</div>
      </div>
    </div>,
    document.body
  );
};

export default Modal;
