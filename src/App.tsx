import { useEffect, useState } from 'react'
import {
  AppBar,
  Avatar,
  Badge,
  Box,
  Button,
  Card,
  Divider,
  IconButton,
  InputBase,
  LinearProgress,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Menu,
  MenuItem,
  Stack,
  Toolbar,
  Typography,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
} from '@mui/material'
import {
  AccountBalanceWalletOutlined,
  AssignmentOutlined,
  ArticleOutlined,
  BusinessOutlined,
  ChevronRightRounded,
  ConstructionOutlined,
  DashboardOutlined,
  DescriptionOutlined,
  FolderOutlined,
  NotificationsNoneRounded,
  PeopleAltOutlined,
  ReceiptLongOutlined,
  SearchRounded,
  SettingsOutlined,
  TuneRounded,
  TrendingUpRounded,
  LogoutRounded,
  PersonRounded,
  ApartmentRounded,
  TimelineRounded,
  CloudUploadOutlined,
  MoreVertRounded,
  GridViewRounded,
  ViewListRounded,
  PictureAsPdfOutlined,
  CreateNewFolderOutlined,
  ArrowBackRounded,
  InboxOutlined,
  LabelOutlined,
  HistoryOutlined,
  ManageSearchOutlined,
  FactCheckOutlined,
  TaskAltOutlined,
  VerifiedOutlined,
} from '@mui/icons-material'
import graditeljLogo from './assets/graditelj_logo_novo.png'
import './App.css'

const modules = [
  {
    title: 'DMS',
    text: '124 nova dokumenta',
    icon: <DescriptionOutlined />,
    tone: 'blue',
  },
  {
    title: 'Financije',
    text: '38 računa na čekanju',
    icon: <ReceiptLongOutlined />,
    tone: 'orange',
  },
  {
    title: 'Gradilišta',
    text: '12 aktivnih projekata',
    icon: <ConstructionOutlined />,
    tone: 'green',
  },
  {
    title: 'Partneri',
    text: '286 evidentiranih partnera',
    icon: <ApartmentRounded />,
    tone: 'violet',
  },
]

const applications = [
  { title: 'Nadzorna ploča', text: 'Pregled poslovanja i najvažnije aktivnosti', icon: <DashboardOutlined />, tone: 'blue', badge: null },
  { title: 'DMS', text: 'Dokumenti, kategorije, verzije i arhiva', icon: <FolderOutlined />, tone: 'blue', badge: '124' },
  { title: 'Financije', text: 'Računi, odobrenja i financijski izvještaji', icon: <AccountBalanceWalletOutlined />, tone: 'orange', badge: '38' },
  { title: 'e-Računi', text: 'Prijem, slanje i praćenje e-računa', icon: <ReceiptLongOutlined />, tone: 'orange', badge: '12' },
  { title: 'Gradilišta', text: 'Projektni troškovi, situacije i gradilišta', icon: <ConstructionOutlined />, tone: 'green', badge: '12' },
  { title: 'Partneri', text: 'Partneri, kontakti i ugovorna dokumentacija', icon: <PeopleAltOutlined />, tone: 'violet', badge: null },
  { title: 'Workflow', text: 'Zadaci, tijekovi odobravanja i statusi', icon: <AssignmentOutlined />, tone: 'violet', badge: '7' },
  { title: 'Izvještaji', text: 'Analitika i upravljački izvještaji', icon: <ArticleOutlined />, tone: 'blue', badge: null },
  { title: 'Identitet i admin', text: 'Korisnici, grupe, uloge i prava pristupa', icon: <SettingsOutlined />, tone: 'green', badge: null },
  { title: 'Integracije', text: 'PANTHEON, posrednik i Nextcloud status', icon: <TimelineRounded />, tone: 'orange', badge: null },
  { title: 'AI Agent', text: 'Lokalni poslovni agent i RAG pretraživanje', icon: <BusinessOutlined />, tone: 'violet', badge: 'Uskoro' },
]

const activities = [
  {
    title: 'Nova verzija dokumenta',
    detail: 'UGOVOR-PARTNER-2026.pdf',
    time: 'Prije 12 min',
    icon: <DescriptionOutlined />,
  },
  {
    title: 'Odobren za isplatu',
    detail: 'Račun #2026-1048',
    time: 'Prije 48 min',
    icon: <ReceiptLongOutlined />,
  },
  {
    title: 'Novi zadatak dodijeljen',
    detail: 'Situacija — Objekt A',
    time: 'Prije 2 sata',
    icon: <AssignmentOutlined />,
  },
]

const dmsCategories = [
  { title: 'Ulazna dokumentacija', count: 86, icon: <InboxOutlined /> },
  { title: 'Razvrstavanje', count: 42, icon: <LabelOutlined /> },
  { title: 'Na odobrenje', count: 14, icon: <FactCheckOutlined /> },
  { title: 'Moji zadaci', count: 7, icon: <TaskAltOutlined /> },
  { title: 'Svi dokumenti', count: 1248, icon: <FolderOutlined /> },
  { title: 'Potpisivanje i komentari', count: 9, icon: <VerifiedOutlined /> },
  { title: 'Pretraživanje', count: null, icon: <ManageSearchOutlined /> },
  { title: 'Arhiva i verzije', count: 364, icon: <HistoryOutlined /> },
]

type DmsCase = {
  id: string
  caseNumber: string
  title: string
  documentTypeCode: string
  projectCode?: string
  status: string
  currentStep?: string
  assignedRole?: string
  assignedTo?: string
  dueAt?: string
  createdAt: string
}

function App() {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null)
  const [activeView, setActiveView] = useState<'apps' | 'dashboard' | 'dms'>('apps')
  const [activeDmsSection, setActiveDmsSection] = useState('Ulazna dokumentacija')
  const isApplicationOpen = activeView !== 'apps'
  const [dmsCases, setDmsCases] = useState<DmsCase[]>([])
  const [createDialogOpen, setCreateDialogOpen] = useState(false)
  const [dmsError, setDmsError] = useState('')
  const [newCaseTitle, setNewCaseTitle] = useState('')
  const [newCaseType, setNewCaseType] = useState('SITUACIJA')
  const [newCaseProject, setNewCaseProject] = useState('')
  const [newCaseRole, setNewCaseRole] = useState('')

  useEffect(() => {
    if (activeView !== 'dms') return

    fetch('http://127.0.0.1:5000/api/dms/cases')
      .then(async (response) => {
        if (!response.ok) throw new Error('DMS API nije dostupan.')
        return response.json() as Promise<DmsCase[]>
      })
      .then(setDmsCases)
      .catch((error: Error) => setDmsError(error.message))
  }, [activeView])

  const refreshDmsCases = async () => {
    const response = await fetch('http://127.0.0.1:5000/api/dms/cases')
    if (!response.ok) throw new Error('DMS API nije dostupan.')
    setDmsCases(await response.json() as DmsCase[])
  }

  const createDmsCase = async () => {
    setDmsError('')
    const response = await fetch('http://127.0.0.1:5000/api/dms/cases', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        title: newCaseTitle,
        documentTypeCode: newCaseType,
        projectCode: newCaseProject || null,
        assignedRole: newCaseRole || null,
        actor: 'Marko Markešić',
      }),
    })

    if (!response.ok) {
      const problem = await response.json()
      setDmsError(problem.title || 'Dokument nije moguće evidentirati.')
      return
    }

    setCreateDialogOpen(false)
    setNewCaseTitle('')
    setNewCaseProject('')
    setNewCaseRole('')
    await refreshDmsCases()
  }

  const transitionDmsCase = async (caseItem: DmsCase, nextStatus: string) => {
    setDmsError('')
    const response = await fetch(`http://127.0.0.1:5000/api/dms/cases/${caseItem.id}/transition`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        status: nextStatus,
        actor: 'Marko Markešić',
        currentStep: nextStatus,
        comment: `Prijelaz putem DMS sučelja: ${caseItem.status} → ${nextStatus}`,
      }),
    })

    if (!response.ok) {
      const problem = await response.json()
      setDmsError(problem.detail || problem.title || 'Prijelaz nije dopušten.')
      return
    }

    await refreshDmsCases()
  }

  return (
    <Box className={activeView === 'apps' ? 'app-shell launcher-shell' : 'app-shell application-shell'}>
      <Box className="main-shell">
        <AppBar className="topbar" elevation={0} position="sticky">
          <Toolbar className="toolbar">
            {activeView === 'apps' && (
              <Box className="launcher-brand">
                <Box className="brand-mark"><img src={graditeljLogo} alt="GSV" /></Box>
                <Box>
                  <Typography className="brand-name">GSV NEXUS</Typography>
                  <Typography className="brand-subtitle">POSLOVNI SUSTAV</Typography>
                </Box>
              </Box>
            )}
            {isApplicationOpen && (
              <IconButton
                aria-label="Natrag na aplikacije"
                className="back-to-apps"
                onClick={() => setActiveView('apps')}
              >
                <ArrowBackRounded />
                <Typography>Natrag na aplikacije</Typography>
              </IconButton>
            )}
            <Box className="search-box">
              <SearchRounded className="search-icon" />
              <InputBase
                fullWidth
                placeholder="Pretraži dokumente, partnere, račune..."
                inputProps={{ 'aria-label': 'Globalno pretraživanje' }}
              />
              <Box className="search-shortcut">Ctrl K</Box>
            </Box>

            <Stack direction="row" spacing={1.25} sx={{ alignItems: 'center' }}>
              <IconButton aria-label="Filtri" className="top-icon">
                <TuneRounded />
              </IconButton>
              <IconButton aria-label="Obavijesti" className="top-icon">
                <Badge variant="dot" color="error">
                  <NotificationsNoneRounded />
                </Badge>
              </IconButton>
              <Divider orientation="vertical" flexItem className="top-divider" />
              <Button
                className="profile-button"
                onClick={(event) => setAnchorEl(event.currentTarget)}
                endIcon={<ChevronRightRounded />}
              >
                <Avatar className="profile-avatar">MM</Avatar>
                <Box className="profile-copy">
                  <Typography>Marko Markešić</Typography>
                  <Typography>Administrator</Typography>
                </Box>
              </Button>
              <Menu
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={() => setAnchorEl(null)}
                className="profile-menu"
              >
                <MenuItem onClick={() => setAnchorEl(null)}>
                  <PersonRounded fontSize="small" /> Moj profil
                </MenuItem>
                <MenuItem onClick={() => setAnchorEl(null)}>
                  <SettingsOutlined fontSize="small" /> Postavke
                </MenuItem>
                <Divider />
                <MenuItem onClick={() => setAnchorEl(null)}>
                  <LogoutRounded fontSize="small" /> Odjava
                </MenuItem>
              </Menu>
            </Stack>
          </Toolbar>
        </AppBar>

        <Box component="main" className="content">
          <Box className="page-heading">
            <Box>
              <Typography className="eyebrow">
                {activeView === 'apps'
                  ? 'GSV NEXUS · POSLOVNE APLIKACIJE'
                  : activeView === 'dms'
                    ? 'GSV NEXUS · DMS'
                    : 'PONEDJELJAK, 24. SRPNJA 2026.'}
              </Typography>
              <Typography component="h1">
                {activeView === 'apps'
                  ? 'Pristupite aplikacijama'
                  : activeView === 'dms'
                    ? 'Dokumentni sustav'
                    : 'Dobro jutro, Marko'}
              </Typography>
            </Box>
            {activeView === 'apps' ? (
              <Button
                className="primary-action"
                startIcon={<DashboardOutlined />}
                onClick={() => setActiveView('dashboard')}
              >
                Nadzorna ploča
              </Button>
            ) : activeView === 'dms' ? (
              <Stack direction="row" spacing={1}>
                <Button
                  className="secondary-action"
                  startIcon={<CreateNewFolderOutlined />}
                >
                  Nova mapa
                </Button>
                <Button
                  className="primary-action"
                  startIcon={<CloudUploadOutlined />}
                  onClick={() => setCreateDialogOpen(true)}
                >
                  Prijenos dokumenta
                </Button>
              </Stack>
            ) : (
              <Button
                className="primary-action"
                startIcon={<DescriptionOutlined />}
                onClick={() => setActiveView('apps')}
              >
                Sve aplikacije
              </Button>
            )}
          </Box>

          {activeView === 'apps' ? (
            <>
              <Box className="app-launcher-heading">
                <Box>
                  <Typography className="launcher-title">Vaše aplikacije</Typography>
                  <Typography className="launcher-subtitle">
                    Dostupne prema vašim korisničkim ovlastima
                  </Typography>
                </Box>
                <Button className="recent-button">Nedavno korišteno <ChevronRightRounded /></Button>
              </Box>
              <Box className="app-launcher-grid">
                {applications.map((application) => (
                  <Card
                    key={application.title}
                    className="app-launcher-card"
                    elevation={0}
                    onClick={() => {
                      if (application.title === 'Nadzorna ploča') setActiveView('dashboard')
                      if (application.title === 'DMS') setActiveView('dms')
                    }}
                  >
                    <Box className={`app-launcher-icon ${application.tone}`}>
                      {application.icon}
                    </Box>
                    <Box className="app-launcher-copy">
                      <Typography className="app-launcher-title">{application.title}</Typography>
                      <Typography className="app-launcher-text">{application.text}</Typography>
                    </Box>
                    {application.badge && (
                      <Box className="app-badge">{application.badge}</Box>
                    )}
                  </Card>
                ))}
              </Box>
            </>
          ) : activeView === 'dms' ? (
            <Box className="dms-layout">
              <Card className="dms-sidebar" elevation={0}>
                <Box className="dms-sidebar-heading">
                  <Typography>DMS</Typography>
                  <IconButton><MoreVertRounded /></IconButton>
                </Box>
                <List disablePadding className="dms-category-list">
                  {dmsCategories.map((category) => (
                    <ListItemButton
                      key={category.title}
                      className={activeDmsSection === category.title ? 'dms-category active' : 'dms-category'}
                      onClick={() => setActiveDmsSection(category.title)}
                    >
                      <ListItemIcon>{category.icon}</ListItemIcon>
                      <ListItemText primary={category.title} />
                      {category.count !== null && (
                        <Typography className="category-count">{category.count}</Typography>
                      )}
                    </ListItemButton>
                  ))}
                </List>
                <Divider />
                <Box className="storage-block">
                  <Box>
                    <Typography className="storage-title">Pohrana dokumenata</Typography>
                    <Typography className="storage-value">68,4 GB od 100 GB</Typography>
                  </Box>
                  <LinearProgress variant="determinate" value={68} />
                </Box>
              </Card>

              <Box className="dms-main">
                <Box className="dms-lifecycle" aria-label="Životni ciklus dokumenta">
                  {['Zaprimanje', 'Evidentiranje', 'Dodjela', 'Obrada', 'Odobravanje', 'Distribucija', 'Arhiva'].map((step, index) => (
                    <Box className={index < 4 ? 'lifecycle-step active' : 'lifecycle-step'} key={step}>
                      <span>{index + 1}</span>
                      <Typography>{step}</Typography>
                    </Box>
                  ))}
                </Box>
                <Box className="dms-stats">
                  <Card elevation={0}>
                    <Box className="dms-stat-icon blue"><FolderOutlined /></Box>
                    <Box><Typography className="dms-stat-value">1.248</Typography><Typography className="dms-stat-label">Ukupno dokumenata</Typography></Box>
                  </Card>
                  <Card elevation={0}>
                    <Box className="dms-stat-icon orange"><CloudUploadOutlined /></Box>
                    <Box><Typography className="dms-stat-value">36</Typography><Typography className="dms-stat-label">Dodano ovaj mjesec</Typography></Box>
                  </Card>
                  <Card elevation={0}>
                    <Box className="dms-stat-icon green"><AssignmentOutlined /></Box>
                    <Box><Typography className="dms-stat-value">14</Typography><Typography className="dms-stat-label">Čeka odobrenje</Typography></Box>
                  </Card>
                </Box>

                <Card className="dms-table-card" elevation={0}>
                  {dmsError && (
                    <Box className="dms-error" role="alert">{dmsError}</Box>
                  )}
                  <Box className="dms-table-toolbar">
                    <Box className="dms-filter-search">
                      <SearchRounded />
                      <InputBase placeholder="Pretraži dokumente" />
                    </Box>
                    <Stack direction="row" spacing={1}>
                      <Button className="dms-filter-button" startIcon={<TuneRounded />}>Filtri</Button>
                      <IconButton className="dms-view-button active-view"><ViewListRounded /></IconButton>
                      <IconButton className="dms-view-button"><GridViewRounded /></IconButton>
                    </Stack>
                  </Box>
                  <Box className="dms-table">
                    <Box className="dms-table-row dms-table-head">
                      <span>NAZIV DOKUMENTA</span>
                      <span>PARTNER / PROJEKT</span>
                      <span>VRIJEME</span>
                      <span>STATUS</span>
                      <span></span>
                    </Box>
                    {dmsCases.map((document) => (
                      <Box className="dms-table-row" key={document.id}>
                        <Box className="dms-document-name">
                          <Box className="document-type pdf">
                            <PictureAsPdfOutlined />
                          </Box>
                          <Box>
                            <Typography>{document.title}</Typography>
                            <Typography>{document.caseNumber} · {document.documentTypeCode}</Typography>
                          </Box>
                        </Box>
                        <Box className="dms-document-partner">
                          <Typography>{document.projectCode || 'Bez projekta'}</Typography>
                          <Typography>{document.assignedTo || document.assignedRole || '-'}</Typography>
                        </Box>
                        <Typography className="dms-document-date">
                          {new Date(document.createdAt).toLocaleDateString('hr-HR')}
                        </Typography>
                        <Box>
                          <span className={`document-status ${document.status === 'Odobreno' ? 'approved' : document.status === 'Vraćeno' || document.status === 'Odbijeno' ? 'pending' : 'draft'}`}>
                            {document.status}
                          </span>
                        </Box>
                        <IconButton
                          className="document-menu"
                          aria-label="Premjesti u obradu"
                          onClick={() => {
                            const nextStatus = document.status === 'Zaprimljeno'
                              ? 'Evidentirano'
                              : document.status === 'Evidentirano'
                                ? 'Dodijeljeno'
                                : document.status === 'Dodijeljeno'
                                  ? 'U obradi'
                                  : document.status === 'U obradi'
                                    ? 'Na odobravanju'
                                    : document.status === 'Na odobravanju'
                                      ? 'Odobreno'
                                      : 'Distribuirano'
                            void transitionDmsCase(document, nextStatus)
                          }}
                        >
                          <ChevronRightRounded />
                        </IconButton>
                      </Box>
                    ))}
                  </Box>
                  <Box className="dms-table-footer">
                    <Typography>Prikazano 1–5 od 1.248 dokumenta</Typography>
                    <Stack direction="row" spacing={0.5}>
                      <Button className="pagination-button">‹</Button>
                      <Button className="pagination-button active">1</Button>
                      <Button className="pagination-button">2</Button>
                      <Button className="pagination-button">3</Button>
                      <Button className="pagination-button">…</Button>
                      <Button className="pagination-button">250</Button>
                      <Button className="pagination-button">›</Button>
                    </Stack>
                  </Box>
                </Card>
              </Box>
            </Box>
          ) : (
            <>
          <Box className="module-grid">
            {modules.map((module) => (
              <Card key={module.title} className="module-card" elevation={0}>
                <Box className={`module-icon ${module.tone}`}>{module.icon}</Box>
                <Box>
                  <Typography className="module-title">{module.title}</Typography>
                  <Typography className="module-text">{module.text}</Typography>
                </Box>
                <IconButton className="card-arrow" aria-label={`Otvori ${module.title}`}>
                  <ChevronRightRounded />
                </IconButton>
              </Card>
            ))}
          </Box>

          <Box className="dashboard-grid">
            <Card className="finance-card" elevation={0}>
              <Box className="card-header">
                <Box>
                  <Typography className="card-title">Financijski pregled</Typography>
                  <Typography className="card-subtitle">Posljednjih 6 mjeseci</Typography>
                </Box>
                <Button className="period-button">Ovaj mjesec <ChevronRightRounded /></Button>
              </Box>
              <Box className="chart-summary">
                <Box>
                  <Typography className="summary-label">Ukupni prihodi</Typography>
                  <Typography className="summary-value">1.284.620,00 €</Typography>
                </Box>
                <Box className="trend-chip">
                  <TrendingUpRounded /> 12,8%
                </Box>
              </Box>
              <Box className="chart" aria-label="Graf prihoda i rashoda">
                <svg viewBox="0 0 680 220" role="img">
                  <g className="grid-lines">
                    <line x1="35" y1="25" x2="660" y2="25" />
                    <line x1="35" y1="75" x2="660" y2="75" />
                    <line x1="35" y1="125" x2="660" y2="125" />
                    <line x1="35" y1="175" x2="660" y2="175" />
                  </g>
                  <path className="income-area" d="M35 170 C90 155 120 142 165 148 C220 155 245 105 290 112 C340 120 365 82 410 92 C460 103 490 55 535 67 C580 78 610 42 660 35 L660 175 L35 175 Z" />
                  <path className="income-line" d="M35 170 C90 155 120 142 165 148 C220 155 245 105 290 112 C340 120 365 82 410 92 C460 103 490 55 535 67 C580 78 610 42 660 35" />
                  <path className="expense-line" d="M35 158 C90 165 120 155 165 160 C220 166 245 140 290 145 C340 150 365 132 410 138 C460 144 490 115 535 122 C580 128 610 108 660 112" />
                  <g className="chart-dots">
                    <circle cx="35" cy="170" r="4" />
                    <circle cx="165" cy="148" r="4" />
                    <circle cx="290" cy="112" r="4" />
                    <circle cx="410" cy="92" r="4" />
                    <circle cx="535" cy="67" r="4" />
                    <circle cx="660" cy="35" r="5" />
                  </g>
                </svg>
                <Box className="chart-labels">
                  <span>Veljača</span><span>Ožujak</span><span>Travanj</span>
                  <span>Svibanj</span><span>Lipanj</span><span>Srpanj</span>
                </Box>
              </Box>
              <Stack direction="row" spacing={3} className="chart-legend">
                <Box><span className="legend-dot income" />Prihodi</Box>
                <Box><span className="legend-dot expense" />Rashodi</Box>
              </Stack>
            </Card>

            <Card className="activity-card" elevation={0}>
              <Box className="card-header">
                <Box>
                  <Typography className="card-title">Najnovije aktivnosti</Typography>
                  <Typography className="card-subtitle">Vaš poslovni sustav</Typography>
                </Box>
                <IconButton className="card-menu"><TuneRounded /></IconButton>
              </Box>
              <List className="activity-list" disablePadding>
                {activities.map((activity) => (
                  <ListItemButton key={activity.title} className="activity-item">
                    <ListItemIcon className="activity-icon">{activity.icon}</ListItemIcon>
                    <ListItemText
                      primary={activity.title}
                      secondary={
                        <Box component="span" className="activity-detail">
                          <span>{activity.detail}</span>
                          <span>{activity.time}</span>
                        </Box>
                      }
                    />
                  </ListItemButton>
                ))}
              </List>
              <Button className="view-all">Pogledaj sve aktivnosti <ChevronRightRounded /></Button>
            </Card>
          </Box>

          <Box className="bottom-grid">
            <Card className="task-card" elevation={0}>
              <Box className="card-header">
                <Box>
                  <Typography className="card-title">Zadaci na čekanju</Typography>
                  <Typography className="card-subtitle">Potrebna vaša odluka</Typography>
                </Box>
                <Typography className="task-count">5</Typography>
              </Box>
              <Box className="task-progress">
                <Box>
                  <Typography>Dovršenost ovog tjedna</Typography>
                  <Typography>68%</Typography>
                </Box>
                <LinearProgress variant="determinate" value={68} />
              </Box>
            </Card>
            <Card className="quick-card" elevation={0}>
              <Typography className="card-title">Brze poveznice</Typography>
              <Box className="quick-links">
                <Button startIcon={<PeopleAltOutlined />}>Novi partner</Button>
                <Button startIcon={<ReceiptLongOutlined />}>Novi račun</Button>
                <Button startIcon={<BusinessOutlined />}>Novo gradilište</Button>
                <Button startIcon={<TimelineRounded />}>Novi izvještaj</Button>
              </Box>
            </Card>
          </Box>
            </>
          )}
        </Box>
      </Box>
      <Dialog
        open={createDialogOpen}
        onClose={() => setCreateDialogOpen(false)}
        fullWidth
        maxWidth="sm"
      >
        <DialogTitle>Novi DMS predmet</DialogTitle>
        <DialogContent sx={{ display: 'grid', gap: 2, pt: '12px !important' }}>
          <TextField
            label="Naziv predmeta"
            value={newCaseTitle}
            onChange={(event) => setNewCaseTitle(event.target.value)}
            fullWidth
            required
          />
          <TextField
            label="Vrsta dokumenta"
            value={newCaseType}
            onChange={(event) => setNewCaseType(event.target.value)}
            fullWidth
            required
          />
          <TextField
            label="Projekt"
            value={newCaseProject}
            onChange={(event) => setNewCaseProject(event.target.value)}
            fullWidth
          />
          <TextField
            label="Odgovorna uloga"
            value={newCaseRole}
            onChange={(event) => setNewCaseRole(event.target.value)}
            fullWidth
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setCreateDialogOpen(false)}>Odustani</Button>
          <Button variant="contained" onClick={createDmsCase} disabled={!newCaseTitle.trim()}>
            Evidentiraj
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}

export default App
