(function () {
  "use strict";

  const select = (el, all = false) => {
    el = el.trim()
    if (all) {
      return [...document.querySelectorAll(el)]
    } else {
      return document.querySelector(el)
    }
  }

  const on = (type, el, listener, all = false) => {
    let selectEl = select(el, all)
    if (selectEl) {
      if (all) {
        selectEl.forEach(e => e.addEventListener(type, listener))
      } else {
        selectEl.addEventListener(type, listener)
      }
    }
  }

  const onscroll = (el, listener) => {
    el.addEventListener('scroll', listener)
  }

  let navbarlinks = select('#navbar .scrollto', true)
  const navbarlinksActive = () => {
    let position = window.scrollY + 200
    navbarlinks.forEach(navbarlink => {
      if (!navbarlink.hash) return
      let section = select(navbarlink.hash)
      if (!section) return
      if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
        navbarlink.classList.add('active')
      } else {
        navbarlink.classList.remove('active')
      }
    })
  }
  window.addEventListener('load', navbarlinksActive)
  onscroll(document, navbarlinksActive)

  const scrollto = (el) => {
    let header = select('#header')
    let offset = header.offsetHeight

    let elementPos = select(el).offsetTop
    window.scrollTo({
      top: elementPos - offset,
      behavior: 'smooth'
    })
  }

  let selectHeader = select('#header')
  let selectTopbar = select('#topbar')
  if (selectHeader) {
    const headerScrolled = () => {
      if (window.scrollY > 100) {
        selectHeader.classList.add('header-scrolled')
        if (selectTopbar) {
          selectTopbar.classList.add('topbar-scrolled')
        }
      } else {
        selectHeader.classList.remove('header-scrolled')
        if (selectTopbar) {
          selectTopbar.classList.remove('topbar-scrolled')
        }
      }
    }
    window.addEventListener('load', headerScrolled)
    onscroll(document, headerScrolled)
  }

  let backtotop = select('.back-to-top')
  if (backtotop) {
    const toggleBacktotop = () => {
      if (window.scrollY > 100) {
        backtotop.classList.add('active')
      } else {
        backtotop.classList.remove('active')
      }
    }
    window.addEventListener('load', toggleBacktotop)
    onscroll(document, toggleBacktotop)
  }

  on('click', '.mobile-nav-toggle', function (e) {
    select('#navbar').classList.toggle('navbar-mobile')
    this.classList.toggle('bi-list')
    this.classList.toggle('bi-x')
  })

  on('click', '.navbar .dropdown > a', function (e) {
    if (select('#navbar').classList.contains('navbar-mobile')) {
      e.preventDefault()
      this.nextElementSibling.classList.toggle('dropdown-active')
    }
  }, true)

  on('click', '.scrollto', function (e) {
    if (select(this.hash)) {
      e.preventDefault()

      let navbar = select('#navbar')
      if (navbar.classList.contains('navbar-mobile')) {
        navbar.classList.remove('navbar-mobile')
        let navbarToggle = select('.mobile-nav-toggle')
        navbarToggle.classList.toggle('bi-list')
        navbarToggle.classList.toggle('bi-x')
      }
      scrollto(this.hash)
    }
  }, true)

  window.addEventListener('load', () => {
    if (window.location.hash) {
      if (select(window.location.hash)) {
        scrollto(window.location.hash)
      }
    }
  });

  let preloader = select('#preloader');
  if (preloader) {
    window.addEventListener('load', () => {
      preloader.remove()
    });
  }

  const glightbox = GLightbox({
    selector: '.glightbox'
  });

  const galelryLightbox = GLightbox({
    selector: '.galelry-lightbox'
  });

  new Swiper('.testimonials-slider', {
    speed: 600,
    loop: true,
    autoplay: {
      delay: 5000,
      disableOnInteraction: false
    },
    slidesPerView: 'auto',
    pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable: true
    },
    breakpoints: {
      320: {
        slidesPerView: 1,
        spaceBetween: 20
      },

      1200: {
        slidesPerView: 2,
        spaceBetween: 20
      }
    }
  });

  new PureCounter();

  var isLoggedIn = false;
  const token = localStorage.getItem('token');
  if (token) {
    isLoggedIn = true;
  }
  var logoutButton = document.getElementById("logoutButton");
  if (isLoggedIn) {
    logoutButton.style.display = "inline-block";
  }

  on('click', '#logoutButton', function (e) {
    e.preventDefault();
    localStorage.removeItem("token");
    window.location.href = 'index.html';
    alert("Logout!");
  })

  on('click', '#moj-nalog', function (e) {
    e.preventDefault();

    var isLoggedIn = false;
    const token = localStorage.getItem('token');
    var userRole = localStorage.getItem('userRole');
    if (token) {
      isLoggedIn = true;
    }

    if (isLoggedIn) {
      console.log('User is logged in');
      if (userRole != 'veterinarian') {
        fetch('https://localhost:7222/api/User/Profile', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        }).then(function (response) {
          if (!response.ok) {
            throw new Error('Error retrieving user profile.');
          }
          return response.json();
        }).then(function (user) {
          if (user.role === 'User') {
            window.location.href = 'userProfile.html';
          } else if (user.role === 'Admin') {
            window.location.href = 'adminProfile.html';
          }
        })
          .catch(function (error) {
            console.log(error);
            alert('Greška prilikom pristupu profila korsnika. Molimo Vas probajte ponovo!');
          });
      }
      else window.location.href = 'vetProfile.html';
    } else {
      console.log('User is not logged in');
      window.location.href = 'login.html';
    }
  });

  window.addEventListener('load', () => {
    var urlParams = new URLSearchParams(window.location.search);
    var petID = urlParams.get('petID');
    var userID = urlParams.get('userID');

    console.log(petID, userID);

    fetch("https://localhost:7222/api/Veterinarian")
      .then(response => response.json())
      .then(data => {
        const vetsContainer = document.getElementById('vetsContainer');
        vetsContainer.innerHTML = '';
        data.forEach(vet => {
          const card = document.createElement('div');
          card.className = 'card mb-4';
          const cardBody = document.createElement('div');
          cardBody.className = 'card-body';
          const vetElement = document.createElement("p");

          fetch("https://localhost:7222/api/Ambulance/" + vet.ambulanceId, {
            method: 'GET',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${token}`
            }
          })
            .then(response => {
              if (!response.ok) {
                throw new Error('Network response was not ok');
              }
              return response.json();
            })
            .then(data => {
              vetElement.textContent = vet.firstName + ' ' + vet.lastName + ' - ' + data.name + ' - ' + data.adress;
            })
            .catch(error => {
              console.error('Error:', error);
            });


          const izaberiDugme = document.createElement("button");
          izaberiDugme.textContent = "Izaberi";
          izaberiDugme.className = "btn btn-primary mb-3 choose-vet";


          const ostaviKomentar = document.createElement("button");
          ostaviKomentar.textContent = "Ostavi komentar";
          ostaviKomentar.className = "btn btn-primary mb-3 comment";

          const prikaziKomentare = document.createElement("button");
          prikaziKomentare.textContent = "Prikazi komentare";
          prikaziKomentare.className = "btn btn-primary mb-3 comment";

          izaberiDugme.addEventListener('click', function (e) {
            e.preventDefault();
            console.log("klik");
            var checkDate = true;
            const selectedDate = document.getElementById("date").value;
            const selectedSymptom = document.getElementById("symptom").value;
            fetch("https://localhost:7222/api/Appointment/veterinarian/" + vet.id, {
              method: "GET"
            })
              .then(response => response.json())
              .then(data => {
                data.forEach(appointment => {

                  const appointmentDateTime = new Date(appointment.date);
                  const selectedDateTime = new Date(selectedDate);
                  console.log(vet.id, petID, selectedDate, selectedSymptom);

                  const timeDifference = Math.abs(selectedDateTime - appointmentDateTime);
                  const timeDifferenceInHours = timeDifference / (1000 * 60 * 60);

                  if (timeDifferenceInHours <= 1) {
                    checkDate = false;
                  }
                });
                if (checkDate == true) {


                  fetch("https://localhost:7222/api/Appointment", {
                    method: "POST",
                    headers: {
                      "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                      "veterinarianId": vet.id,
                      "petId": petID,
                      "date": selectedDate,
                      "symptom": selectedSymptom
                    })
                  }).then(p => {
                    if (p.ok) {
                      alert("Uspesno ste zakazali temrin");
                    }
                    else {
                      alert("Greska prilikom zakazivanja termina");
                    }
                  })
                }
                else { alert("Termin koji ste izabrali je zauzet, izaberite drugi termin ili lekara!"); }
              })
          });

          const addCommentsContainer = document.createElement('div');
          addCommentsContainer.id = 'addCommentsContainer';
          addCommentsContainer.style.display = 'none';
          addCommentsContainer.innerHTML = '';
          let formaPrikazana = false;
          ostaviKomentar.addEventListener("click", function (e) {
            e.preventDefault();
            addCommentsContainer.style.display = (addCommentsContainer.style.display === 'none') ? 'block' : 'none';
          });

          if (formaPrikazana) {
            addCommentsContainer.firstChild.remove();
          }
          const forma = document.createElement("form");
          forma.className = "formaa";


          const veterinarianIdInput = document.createElement("input");
          veterinarianIdInput.setAttribute("type", "hidden");
          veterinarianIdInput.setAttribute("name", "veterinarianId");
          veterinarianIdInput.value = vet.id;

          const userIdInput = document.createElement("input");
          userIdInput.setAttribute("type", "hidden");
          userIdInput.setAttribute("name", "userId");
          userIdInput.value = userID;

          const dateInput = document.createElement("input");
          dateInput.setAttribute("type", "hidden");
          dateInput.setAttribute("name", "date");
          dateInput.value = new Date().toISOString();

          const commentInput = document.createElement("input");
          commentInput.className = "form-control";
          commentInput.setAttribute("type", "text");
          commentInput.setAttribute("name", "comment");
          commentInput.setAttribute("placeholder", "Unesite komentar");

          const submitButton = document.createElement("button");
          submitButton.className = 'btn btn-primary mb-3 submit';
          submitButton.setAttribute("type", "submit");
          submitButton.textContent = "Postavi";

          forma.appendChild(veterinarianIdInput);
          forma.appendChild(userIdInput);
          forma.appendChild(dateInput);
          forma.appendChild(commentInput);
          forma.appendChild(submitButton);

          forma.addEventListener("submit", (event) => {
            event.preventDefault();
            const komentar = {
              veterinarianId: Number(forma.veterinarianId.value),
              userId: Number(forma.userId.value),
              date: forma.date.value,
              comment: forma.comment.value
            };

            fetch("https://localhost:7222/api/Review", {
              method: "POST",
              headers: {
                "Content-Type": "application/json"
              },
              body: JSON.stringify(komentar)
            })
              .then(response => response.json())
              .then(data => {
                console.log("Komentar uspešno poslat:", data);

              })
              .catch(error => {
                console.error("Greška prilikom slanja komentara:", error);

              });

            forma.remove();
            formaPrikazana = false;
          });

          addCommentsContainer.appendChild(forma);
          formaPrikazana = true;

          const commentsContainer = document.createElement('div');
          commentsContainer.id = 'commentsContainer';
          commentsContainer.style.display = 'none';
          commentsContainer.innerHTML = '';

          prikaziKomentare.addEventListener('click', function (e) {
            e.preventDefault();
            commentsContainer.style.display = (commentsContainer.style.display === 'none') ? 'block' : 'none';
          });

          fetch("https://localhost:7222/api/Review/veterinarian/" + vet.id, {
            headers: {
              'Authorization': `Bearer ${token}`
            }
          })
            .then(response => response.json())
            .then(data => {
              if (data != null) {
                data.forEach(review => {

                  const commentDiv = document.createElement('div');
                  commentDiv.classList.add('card', 'mb-4');

                  const commentCardBody = document.createElement('div');
                  commentCardBody.classList.add('card-body');

                  const commentText = document.createElement('p');
                  commentText.textContent = review.comment;

                  const commentAuthor = document.createElement('div');
                  commentAuthor.classList.add('d-flex', 'flex-row', 'align-items-center');

                  const commentAuthorImage = document.createElement('img');
                  commentAuthorImage.src = "assets/img/user.jpg"
                  commentAuthorImage.alt = 'avatar';
                  commentAuthorImage.width = 25;
                  commentAuthorImage.height = 25;

                  const commentAuthorName = document.createElement('p');
                  commentAuthorName.classList.add('small', 'mb-0', 'ms-2');

                  fetch("https://localhost:7222/api/User/" + review.userId, {
                    headers: {
                      'Authorization': `Bearer ${token}`
                    }
                  })
                    .then(response => response.json())
                    .then(data => {
                      commentAuthorName.textContent = data.firstName + ' ' + data.lastName;
                    })
                    .catch(error => console.error('Error fetching comentator informarion:', error));

                  commentAuthor.appendChild(commentAuthorImage);
                  commentAuthor.appendChild(commentAuthorName);

                  commentCardBody.appendChild(commentText);
                  commentCardBody.appendChild(commentAuthor);

                  commentDiv.appendChild(commentCardBody);
                  commentsContainer.appendChild(commentDiv);
                  cardBody.appendChild(commentsContainer);

                });

              }
              else alert('Veterinar nema komentara!');
            })
            .catch(error => console.error('Error fetching reviews:', error));


          cardBody.appendChild(vetElement);
          cardBody.appendChild(izaberiDugme);
          cardBody.appendChild(ostaviKomentar);
          cardBody.appendChild(prikaziKomentare);
          cardBody.appendChild(addCommentsContainer);
          card.appendChild(cardBody);
          vetsContainer.appendChild(card);

        });
      })
      .catch(error => {
        console.error('Error:', error);
      });

  });

})()