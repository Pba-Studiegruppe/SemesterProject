// ─────────────────────────────────────────────────────────────────────────────
// Tiny rich-text editor for Blazor.
// Wires a contenteditable div to a toolbar that uses document.execCommand,
// then notifies Blazor whenever content changes (debounced).
//
// execCommand is technically deprecated but still implemented everywhere we
// care about. Swap for a richer library (Quill, ProseMirror, TipTap) later
// without changing the C# component contract.
// ─────────────────────────────────────────────────────────────────────────────
window.assignRichText = (function () {

    function setupPlaceholder(editorEl) {
        const update = () => {
            const isEmpty =
                editorEl.innerHTML === '' ||
                editorEl.innerHTML === '<br>' ||
                editorEl.innerHTML === '<div><br></div>';
            editorEl.classList.toggle('is-empty', isEmpty);
        };
        update();
        editorEl.addEventListener('input', update);
        editorEl.addEventListener('blur', update);
    }

    function init(toolbarEl, editorEl, dotnetRef, initialValue) {
        if (!toolbarEl || !editorEl) return;
        if (initialValue) editorEl.innerHTML = initialValue;

        setupPlaceholder(editorEl);

        let debounceTimer;
        const notifyChange = () => {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => {
                dotnetRef.invokeMethodAsync('OnContentChanged', editorEl.innerHTML);
            }, 200);
        };

        // Toolbar buttons
        toolbarEl.querySelectorAll('[data-cmd]').forEach(btn => {
            // Stop focus stealing from the editor.
            btn.addEventListener('mousedown', e => e.preventDefault());
            btn.addEventListener('click', () => {
                const cmd = btn.dataset.cmd;
                const arg = btn.dataset.arg || null;
                try {
                    document.execCommand(cmd, false, arg);
                } catch (_) { /* ignore */ }
                editorEl.focus();
                notifyChange();
            });
        });

        editorEl.addEventListener('input', notifyChange);
        editorEl.addEventListener('blur', notifyChange);

        // Keyboard shortcuts.
        editorEl.addEventListener('keydown', (e) => {
            if ((e.ctrlKey || e.metaKey) && !e.shiftKey) {
                if (e.key === 'b') { e.preventDefault(); document.execCommand('bold'); notifyChange(); }
                else if (e.key === 'i') { e.preventDefault(); document.execCommand('italic'); notifyChange(); }
                else if (e.key === 'u') { e.preventDefault(); document.execCommand('underline'); notifyChange(); }
            }
        });

        // Strip styled paste so it inherits our editor styles.
        editorEl.addEventListener('paste', (e) => {
            e.preventDefault();
            const text = (e.clipboardData || window.clipboardData).getData('text/plain');
            document.execCommand('insertText', false, text);
        });
    }

    return { init };
})();
