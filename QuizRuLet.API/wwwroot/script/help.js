document.addEventListener('keydown', function(event) {
    if (event.key === 'F1') {
      event.preventDefault();
      window.open(helpPage, "_blank");
    }
  });