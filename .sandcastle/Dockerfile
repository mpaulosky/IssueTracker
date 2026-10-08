FROM node:22-bookworm

# Install system dependencies
RUN apt-get update && apt-get install -y \
  git \
  curl \
  jq \
  libicu72 \
  python3 \
  python3-pip \
  xz-utils \
  && rm -rf /var/lib/apt/lists/*

# The linters .sandcastle/check.sh runs, at the versions CI runs: shellcheck as
# pinned in .github/workflows/lint-actions.yml (Debian's is 0.9.0, which misses
# newer checks), and yamllint at a current release rather than Debian's 1.29.
ARG SHELLCHECK_VERSION=v0.11.0
ARG YAMLLINT_VERSION=1.38.0
RUN case "$(uname -m)" in \
    x86_64) arch=x86_64; sha=8c3be12b05d5c177a04c29e3c78ce89ac86f1595681cab149b65b97c4e227198 ;; \
    aarch64) arch=aarch64; sha=12b331c1d2db6b9eb13cfca64306b1b157a86eb69db83023e261eaa7e7c14588 ;; \
    *) echo "No shellcheck build for $(uname -m)" >&2; exit 1 ;; \
  esac \
  && curl -fsSL -o /tmp/shellcheck.tar.xz \
    "https://github.com/koalaman/shellcheck/releases/download/${SHELLCHECK_VERSION}/shellcheck-${SHELLCHECK_VERSION}.linux.${arch}.tar.xz" \
  && echo "${sha}  /tmp/shellcheck.tar.xz" | sha256sum -c - \
  && tar -xJf /tmp/shellcheck.tar.xz -C /tmp \
  && install -m 0755 "/tmp/shellcheck-${SHELLCHECK_VERSION}/shellcheck" /usr/local/bin/shellcheck \
  && rm -rf /tmp/shellcheck.tar.xz "/tmp/shellcheck-${SHELLCHECK_VERSION}" \
  && pip3 install --no-cache-dir --break-system-packages "yamllint==${YAMLLINT_VERSION}"

# Install GitHub CLI
RUN curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg \
  | dd of=/usr/share/keyrings/githubcli-archive-keyring.gpg \
  && echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" \
  | tee /etc/apt/sources.list.d/github-cli.list > /dev/null \
  && apt-get update && apt-get install -y gh \
  && rm -rf /var/lib/apt/lists/*

# Build-args for UID/GID alignment: sandcastle docker build-image
# defaults these to the host user's UID/GID so image-built files
# and bind-mounted files share an owner without runtime chown.
ARG AGENT_UID=1000
ARG AGENT_GID=1000

# Rename the base image's "node" user to "agent" and align UID/GID.
RUN groupmod -o -g $AGENT_GID node && usermod -o -u $AGENT_UID -g $AGENT_GID -d /home/agent -m -l agent node

# pnpm through corepack, at the version package.json's "packageManager" pins.
RUN corepack enable pnpm
ENV COREPACK_ENABLE_DOWNLOAD_PROMPT=0
USER ${AGENT_UID}:${AGENT_GID}

# Install Claude Code CLI
RUN curl -fsSL https://claude.ai/install.sh | bash

# Install the .NET SDK. global.json pins 10.0.401 with rollForward latestMinor,
# so the latest 10.0 SDK satisfies it.
RUN curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh \
  && bash /tmp/dotnet-install.sh --channel 10.0 --install-dir /home/agent/.dotnet \
  && rm /tmp/dotnet-install.sh

ENV DOTNET_ROOT="/home/agent/.dotnet" \
  DOTNET_CLI_TELEMETRY_OPTOUT=1 \
  DOTNET_NOLOGO=1

# Add Claude and .NET to PATH
ENV PATH="/home/agent/.local/bin:/home/agent/.dotnet:/home/agent/.dotnet/tools:$PATH"

WORKDIR /home/agent

# In worktree sandbox mode, Sandcastle bind-mounts the git worktree at /home/agent/workspace
# and overrides the working directory to /home/agent/workspace at container start.
# Structure your Dockerfile so that /home/agent/workspace can serve as the project root.
ENTRYPOINT ["sleep", "infinity"]
